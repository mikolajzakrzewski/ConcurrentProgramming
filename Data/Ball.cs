using System.Diagnostics;
using System.Numerics;

namespace Data;

internal class Ball : DataApi, IObservable<DataApi>
{
    private readonly List<IObserver<DataApi>> _observers = [];
    private readonly object _positionLock = new();
    private readonly object _stopLock = new();
    private readonly object _velocityLock = new();
    private bool _isStopped;
    private Vector2 _position;
    private Vector2 _velocity;
    private Logger _logger;
    private readonly object _loggerLock = new();

    public Ball(Vector2 position, int radius, float velocity, Random random)
    {
        _position = position;
        _logger = new Logger();
        Radius = radius;
        Task.Run(() => Move(velocity, random));
    }

    public override Vector2 Position
    {
        get
        {
            lock (_positionLock)
            {
                return _position;
            }
        }
    }

    public override Vector2 Velocity
    {
        get
        {
            lock (_velocityLock)
            {
                return _velocity;
            }
        }
        set
        {
            lock (_velocityLock)
            {
                _velocity = value;
            }
        }
    }

    public override int Radius { get; }

    public override bool IsStopped
    {
        set
        {
            lock (_stopLock)
            {
                _isStopped = value;
            }
        }
    }

    public override IDisposable Subscribe(IObserver<DataApi> observer)
    {
        if (!_observers.Contains(observer)) _observers.Add(observer);
        return new SubscriptionToken(_observers, observer);
    }

    private async void Move(float velocity, Random random)
    {
        Stopwatch stopwatch = new Stopwatch();
        float moveAngle = random.Next(0, 360);
        Velocity = new Vector2(velocity * (float)Math.Cos(moveAngle), velocity * (float)Math.Sin(moveAngle));

        stopwatch.Start();
        float lastUpdateTime = 0f;

        while (true)
        {
            float currentTime = (float)stopwatch.Elapsed.TotalSeconds;
            float elapsedSinceLastUpdate = currentTime - lastUpdateTime;

            if (elapsedSinceLastUpdate >= 1f / 60f)
            {
                lock (_positionLock)
                {
                    _position += Velocity * elapsedSinceLastUpdate;
                }

                NotifyObservers(this);

                lastUpdateTime = currentTime;

                lock (_loggerLock) 
                {
                    _logger.Add(this, DateTime.Now.ToString());
                }
            }

            lock (_stopLock)
            {
                if (_isStopped)
                {
                    stopwatch.Stop();
                    break;
                }
            }

            //TODO: Ogarnąć czy może tak być.
            await Task.Yield();
        }
    }

    private void NotifyObservers(DataApi ball)
    {
        foreach (var observer in _observers) observer.OnNext(ball);
    }
}

internal class SubscriptionToken(ICollection<IObserver<DataApi>> observers, IObserver<DataApi> observer)
    : IDisposable
{
    public void Dispose()
    {
        observers.Remove(observer);
    }
}