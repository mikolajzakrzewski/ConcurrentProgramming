using System;

namespace Data
{
    public class Ball : DataAPI, IObservable<Tuple<float, float>>
    {
        private float _x;
        private float _y;
        private readonly int _radius;
        private readonly object _moveLock = new object();
        private List<IObserver<Tuple<float, float>>> _observers;

        public Ball(float x, float y, int radius)
        {
            this._x = x;
            this._y = y;
            this._radius = radius;
            _observers = new List<IObserver<Tuple<float, float>>>();
        }

        public override float X
        {
            get => _x;
            set
            {
                if (_x != value)
                {
                    _x = value;
                    NotifyObservers();
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
                    NotifyObservers();
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
                }

                NotifyObservers();
            }
        }

        public IDisposable? Subscribe(IObserver<Tuple<float, float>> observer)
        {
            if (!_observers.Contains(observer))
            {
                var subscriptionToken = new SubscriptionToken(_observers, observer);
                _observers.Add(observer);
                return subscriptionToken;
            }
            return null;
        }

        private void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new Tuple<float, float>(_x, _y));
            }
        }
    }

    public class SubscriptionToken : IDisposable
    {
        private List<IObserver<Tuple<float, float>>> _observers;
        private IObserver<Tuple<float, float>> _observer;

        public SubscriptionToken(List<IObserver<Tuple<float, float>>> observers, IObserver<Tuple<float, float>> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}
