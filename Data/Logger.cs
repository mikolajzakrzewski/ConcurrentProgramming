using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

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
                _bufferOverflowed = true;
        });
    }

    private async void Write()
    {
        await using StreamWriter streamWriter = new("log.json", append: false, encoding: Encoding.UTF8);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        while (true)
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
            var log = JsonSerializer.Serialize(dto, options);
            await streamWriter.WriteLineAsync(log);
            await streamWriter.FlushAsync();
        }
    }
}