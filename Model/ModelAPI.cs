using System.Collections.ObjectModel;
using Logic;

namespace Model;

public abstract class ModelApi
{
    public abstract int Width { get; }

    public abstract int Height { get; }

    public abstract ObservableCollection<BallModelApi> Balls { get; }

    public static ModelApi Instance()
    {
        return new Model();
    }

    public static ModelApi Instance(LogicApi table)
    {
        return new Model(table);
    }

    public abstract void CreateBalls(int number, int radius);

    public abstract void Start(float velocity);

    public abstract void ResetTable();
}