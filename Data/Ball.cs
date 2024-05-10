using System.Diagnostics;
using System.Numerics;

namespace Data;

internal class Ball(Vector2 position, int radius, int mass) : DataApi, IObservable<DataApi>
{
    private readonly object _moveLock = new();
    private readonly List<IObserver<DataApi>> _observers = [];
    private readonly object _velocityLock = new();
    private Vector2 _velocity;

    public override Vector2 Position
    {
        get => position;
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

    public override int Radius { get; } = radius;

    public override int Mass { get; } = mass;

    public override IDisposable Subscribe(IObserver<DataApi> observer)
    {
        if (!_observers.Contains(observer)) _observers.Add(observer);
        return new SubscriptionToken(_observers, observer);
    }

    public override async Task Move(float velocity)
    {
        Random rand = new();
        float moveAngle = rand.Next(0, 360);
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
            lock (_moveLock)
            {
                position += velocityChange;
            }

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