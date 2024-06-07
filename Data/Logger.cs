using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace Data;

internal class Logger
{
    private const int MaxBufferSize = 1000;
    private static Logger? _instance;
    private static readonly object LoggerLock = new();
    private static readonly object OverflowLock = new();
    private readonly BlockingCollection<BallDto> _queue;
    private bool _bufferOverflowed;

    private Logger()
    {
        _queue = new BlockingCollection<BallDto>(new ConcurrentQueue<BallDto>(), MaxBufferSize);
        Task.Run(Write);
    }

    public static Logger Instance()
    {
        lock (LoggerLock)
        {
            if (_instance != null) return _instance;
            _instance = new Logger();
            return _instance;
        }
    }

    public void Add(DataApi ball, DateTime date)
    {
        Task.Run(() =>
        {
            var addSucceeded = _queue.TryAdd(new BallDto(ball.Id, ball.Position, ball.Velocity, date));
            if (addSucceeded) return;
            lock (OverflowLock)
            {
                _bufferOverflowed = true;
            }
        });
    }

    private async void Write()
    {
        await using StreamWriter streamWriter = new("log.json", false, Encoding.UTF8);
        var settings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
            Formatting = Formatting.Indented
        };

        while (!_queue.IsCompleted)
        {
            var overflowOccured = false;
            lock (OverflowLock)
            {
                if (_bufferOverflowed)
                {
                    overflowOccured = true;
                    _bufferOverflowed = false;
                }
            }

            if (overflowOccured) await streamWriter.WriteLineAsync("Buffer overflow occurred!");

            var dto = _queue.Take();
            var log = JsonConvert.SerializeObject(dto, settings);
            await streamWriter.WriteLineAsync(log);
            await streamWriter.FlushAsync();
        }
    }
}