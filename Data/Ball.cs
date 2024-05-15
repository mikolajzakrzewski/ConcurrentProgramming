using System.Diagnostics;
using System.Numerics;

namespace Data;

internal class Ball : DataApi, IObservable<DataApi>
{
    private readonly List<IObserver<DataApi>> _observers = [];
    private readonly object _positionLock = new();
    private readonly object _velocityLock = new();
    private Vector2 _position;
    private Vector2 _velocity;

    public Ball(Vector2 position, int radius, float velocity, Random random)
    {
        _position = position;
        Radius = radius;
        Task.Run(() => { _ = Move(velocity, random); });
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
        get => _velocity;
        set
        {
            lock (_velocityLock)
            {
                _velocity = value;
            }
        }
    }

    public override int Radius { get; }

    public override IDisposable Subscribe(IObserver<DataApi> observer)
    {
        if (!_observers.Contains(observer)) _observers.Add(observer);
        return new SubscriptionToken(_observers, observer);
    }

    private async Task Move(float velocity, Random random)
    {
        float moveAngle = random.Next(0, 360);
        Velocity = new Vector2(velocity * (float)Math.Cos(moveAngle), velocity * (float)Math.Sin(moveAngle));
        const float timeOfTravel = 1f / 60f;
        while (true)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            await Task.Delay(TimeSpan.FromSeconds(timeOfTravel));
            stopwatch.Stop();
            var timeElapsed = (float)stopwatch.Elapsed.TotalSeconds;
            var velocityChange = Velocity * timeElapsed;
            _position += velocityChange;
            NotifyObservers(this);
        }
    }

    public void NotifyObservers(DataApi ball)
    {
        foreach (var observer in _observers) observer.OnNext(ball);
    }
}

public class SubscriptionToken(ICollection<IObserver<DataApi>> observers, IObserver<DataApi> observer)
    : IDisposable
{
    public void Dispose()
    {
        if (observer != null && observers.Contains(observer)) observers.Remove(observer);
    }
}