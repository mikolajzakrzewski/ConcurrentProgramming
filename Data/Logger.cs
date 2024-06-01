using System.Collections.Concurrent;
using System.Text.Json;

namespace Data;

internal class Logger
{
    private const int MaxBufferSize = 1000;
    private static Logger? _instance;
    private static readonly object LoggerLock = new();
    private static readonly object OverflowLock = new();
    private readonly ConcurrentQueue<BallDto> _queue;
    private bool _bufferOverflowed;

    public Logger()
    {
        _queue = new ConcurrentQueue<BallDto>();
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

    public void Add(DataApi ball, string date)
    {
        Task.Run(() =>
        {
            if (_queue.Count >= MaxBufferSize)
                lock (OverflowLock)
                {
                    _bufferOverflowed = true;
                }
            else
                _queue.Enqueue(new BallDto(ball.Id, ball.Position, ball.Velocity, date));
        });
    }

    private async void Write()
    {
        await using StreamWriter streamWriter = new("log.json");
        while (true)
        {
            while (_queue.TryDequeue(out var dto))
            {
                var log = JsonSerializer.Serialize(dto);
                await streamWriter.WriteLineAsync(log);
            }

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

            await streamWriter.FlushAsync();
        }
    }
}