using System.Collections.Concurrent;
using System.Text.Json;

namespace Data
{
    internal class Logger
    {
        ConcurrentQueue<BallDto> _queue;
        public Logger() 
        { 
            _queue = new ConcurrentQueue<BallDto>();
            Write();
        }

        public void Add(DataApi ball, string date) 
        { 
            _queue.Enqueue(new BallDto(ball.Position, ball.Velocity, date));
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
                        streamWriter.WriteLine(log);
                    }
                    await streamWriter.FlushAsync();
                }
            });
        }
    }
}
