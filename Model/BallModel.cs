using System.ComponentModel;
using System.Numerics;

namespace Model;

internal class BallModel(Vector2 position, int radius) : BallModelApi, INotifyPropertyChanged
{
    public override float X
    {
        get => position.X;
        set
        {
            position.X = value;
            OnPropertyChanged(nameof(X));
        }
    }

    public override float Y
    {
        get => position.Y;
        set
        {
            position.Y = value;
            OnPropertyChanged(nameof(Y));
        }
    }

    public override int Radius => radius;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}