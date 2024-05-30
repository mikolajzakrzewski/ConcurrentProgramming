using System.Collections.Concurrent;
using System.Text.Json;

namespace Data
{
    internal class Logger
    {
        private static Logger? _instance;
        private static readonly object _loggerLock = new object();
        private ConcurrentQueue<BallDto> _queue;
        private readonly int _maxBufferSize = 1000;
        private bool _bufferOverflowed = false;

        public Logger() 
        { 
            _queue = new ConcurrentQueue<BallDto>();
            Write();
        }

        public static Logger Instance()
        {
            lock (_loggerLock)
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
                return _instance;
            }
        }

        public void Add(DataApi ball, string date) 
        {
            _queue.Enqueue(new BallDto(ball.Id, ball.Position, ball.Velocity, date));

            if (_queue.Count >= _maxBufferSize)
            {
                _bufferOverflowed = true;
            }
        }

        private void Write() 
        {
            Task.Run(async () => {
                using StreamWriter streamWriter = new StreamWriter("log.json");
                while (true) 
                {
                    while (_queue.TryDequeue(out BallDto dto))
                    {
                        string log = JsonSerializer.Serialize(dto);
                        await streamWriter.WriteLineAsync(log);
                    }

                    if (_bufferOverflowed)
                    {
                        await streamWriter.WriteLineAsync("Buffer overflow occurred!");
                        _bufferOverflowed = false;
                    }

                    await streamWriter.FlushAsync();
                }
            });
        }
    }
}
