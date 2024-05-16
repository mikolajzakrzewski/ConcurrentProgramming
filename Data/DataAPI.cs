using System.Numerics;

namespace Data;

public abstract class DataApi : IObservable<DataApi>
{
    public abstract Vector2 Position { get; }

    public abstract Vector2 Velocity { get; set; }

    public abstract int Radius { get; }

    public abstract IDisposable Subscribe(IObserver<DataApi> observer);

    public abstract void Stop();

    public static DataApi Instance(Vector2 position, int radius, float velocity, Random random)
    {
        return new Ball(position, radius, velocity, random);
    }
}