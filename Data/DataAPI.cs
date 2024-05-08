using System.Numerics;

namespace Data;

public abstract class DataApi : IObservable<DataApi>
{
    public abstract Vector2 Position { get; set; }

    public abstract Vector2 Velocity { get; set; }

    public abstract int Radius { get; }

    public abstract int Mass { get; }

    public abstract IDisposable Subscribe(IObserver<DataApi> observer);

    public static DataApi Instance(Vector2 position, int radius, int mass)
    {
        return new Ball(position, radius, mass);
    }

    public abstract Task Move(float velocity);
}