using System.Numerics;

namespace Model;

public abstract class BallModelApi
{
    public abstract float X { get; set; }

    public abstract float Y { get; set; }

    public abstract int Radius { get; }

    public static BallModelApi Instance(Vector2 position, int radius)
    {
        return new BallModel(position, radius);
    }
}