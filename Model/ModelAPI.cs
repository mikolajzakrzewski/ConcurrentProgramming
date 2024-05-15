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

    public abstract void Start(int number, int radius, float velocity);

    public abstract void ResetTable();
}