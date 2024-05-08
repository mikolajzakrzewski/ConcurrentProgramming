using Data;

namespace Logic;

public abstract class LogicApi : IObservable<LogicApi>
{
    public abstract int Width { get; }

    public abstract int Height { get; }

    public abstract IDisposable Subscribe(IObserver<LogicApi> observer);

    public static LogicApi Instance(int width, int height)
    {
        return new Table(width, height);
    }

    public static LogicApi Instance(int width, int height, List<DataApi> balls)
    {
        return new Table(width, height, balls);
    }

    public abstract List<List<float>> GetBallPositions();

    public abstract void CreateBalls(int number, int radius);

    public abstract void Start(float velocity);

    public abstract void ResetTable();
}