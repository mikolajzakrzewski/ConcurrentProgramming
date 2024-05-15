using System.Numerics;

namespace Data;

public abstract class DataApi : IObservable<DataApi>
{
    public abstract Vector2 Position { get; }

    public abstract Vector2 Velocity { get; set; }

    public abstract int Radius { get; }

    public abstract IDisposable Subscribe(IObserver<DataApi> observer);

    public static DataApi Instance(Vector2 position, int radius)
    {
        return new Ball(position, radius);
    }

    public abstract Task Move(float velocity, Random random);
}