using System.ComponentModel;

namespace Data
{
    public class Ball : DataAPI, INotifyPropertyChanged
    {
        private float _x;
        private float _y;
        private readonly int _radius;
        private readonly object _moveLock = new object();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string  propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Ball(float x, float y, int radius)
        {
            this._x = x;
            this._y = y;
            this._radius = radius;
        }

        public override float X
        {
            get => _x;
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        public override float Y
        {
            get => _y;
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }

        public override int Radius
        {
            get => _radius;
        }

        public override async Task Move(float x, float y, double velocity)
        {
            float xDiff = Math.Abs(_x - x);
            float yDiff = Math.Abs(_y - y);
            float xTick = 1;
            float yTick = 1;
            if (xDiff > yDiff)
            {
                yTick = yDiff / xDiff;
            }
            else if (yDiff > xDiff)
            {
                xTick = xDiff / yDiff;
            }
            double distanceTravelled;
            float currentX = _x;
            float currentY = _y;
            while (currentX != x || currentY != y)
            {
                if (currentX < x)
                {
                    if (currentX + xTick > x)
                    {
                        currentX = x;
                    }
                    else
                    {
                        currentX += xTick;
                    }
                }
                else if (currentX > x)
                {
                    if (currentX - xTick < x)
                    {
                        currentX = x;
                    }
                    else
                    {
                        currentX -= xTick;
                    }
                }
                if (currentY < y)
                {
                    if (currentY + yTick > y)
                    {
                        currentY = y;
                    }
                    else
                    {
                        currentY += yTick;
                    }
                }
                else if (currentY > y)
                {
                    if (currentY - yTick < y)
                    {
                        currentY = y;
                    }
                    else
                    {
                        currentY -= yTick;
                    }
                }
                distanceTravelled = Math.Sqrt(Math.Pow(Math.Abs(_x - currentX), 2) + Math.Pow(Math.Abs(_y - currentY), 2));
                double timeOfTravel = distanceTravelled / velocity;
                await Task.Delay(TimeSpan.FromSeconds(timeOfTravel));
                lock (_moveLock)
                {
                    Console.WriteLine($"Ball moved to {currentX}, {currentY}");
                    _x = currentX;
                    _y = currentY;
                    OnPropertyChanged(nameof(X));
                    OnPropertyChanged(nameof(Y));
                }
            }
        }
    }
}
