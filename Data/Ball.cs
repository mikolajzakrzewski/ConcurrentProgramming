using System;

namespace Data
{
    public class Ball : DataAPI, IObservable<Ball>
    {
        private float _x;
        private float _y;
        private float _velociyX;
        private float _velocityY;
        private readonly int _radius;
        private readonly object _moveLock = new object();
        private List<IObserver<Ball>> _observers;

        public Ball(float x, float y, int radius)
        {
            this._x = x;
            this._y = y;
            this._radius = radius;
            this._observers = new List<IObserver<Ball>>();
        }

        public override float X
        {
            get => _x;
            set
            {
                if (_x != value)
                {
                    _x = value;
                    NotifyObservers(this);
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
                    NotifyObservers(this);
                }
            }
        }

        public override float VelocityX
        {
            get => _velociyX;
            set
            {
                if (_velociyX != value)
                {
                    _velociyX = value;
                    NotifyObservers(this);
                }
            }
        }

        public override float VelocityY
        {
            get => _velocityY;
            set
            {
                if (_velocityY != value)
                {
                    _velocityY = value;
                    NotifyObservers(this);
                }
            }
        }

        public override int Radius
        {
            get => _radius;
        }

        public override async Task Move(float velocity)
        {
            var rand = new Random();
            float moveAngle = rand.Next(0, 360);
            VelocityX = velocity * (float)Math.Cos(moveAngle);
            VelocityY = velocity * (float)Math.Sin(moveAngle);
            float timeOfTravel = 0.01f;
            while(true)
            {
                float xChange = VelocityX * timeOfTravel;
                float yChange = VelocityY * timeOfTravel;
                await Task.Delay(TimeSpan.FromSeconds(timeOfTravel));
                lock (_moveLock)
                {
                    _x = _x + xChange;
                    _y = _y + yChange;
                    NotifyObservers(this);
                }
            }
        }

        //public override async Task Move(float x, float y, double velocity)
        //{
        //    float xDiff = Math.Abs(_x - x);
        //    float yDiff = Math.Abs(_y - y);
        //    float xTick = 1;
        //    float yTick = 1;
        //    if (xDiff > yDiff)
        //    {
        //        yTick = yDiff / xDiff;
        //    }
        //    else if (yDiff > xDiff)
        //    {
        //        xTick = xDiff / yDiff;
        //    }
        //    double distanceTravelled;
        //    float currentX = _x;
        //    float currentY = _y;
        //    while (currentX != x || currentY != y)
        //    {
        //        if (currentX < x)
        //        {
        //            if (currentX + xTick > x)
        //            {
        //                currentX = x;
        //            }
        //            else
        //            {
        //                currentX += xTick;
        //            }
        //        }
        //        else if (currentX > x)
        //        {
        //            if (currentX - xTick < x)
        //            {
        //                currentX = x;
        //            }
        //            else
        //            {
        //                currentX -= xTick;
        //            }
        //        }
        //        if (currentY < y)
        //        {
        //            if (currentY + yTick > y)
        //            {
        //                currentY = y;
        //            }
        //            else
        //            {
        //                currentY += yTick;
        //            }
        //        }
        //        else if (currentY > y)
        //        {
        //            if (currentY - yTick < y)
        //            {
        //                currentY = y;
        //            }
        //            else
        //            {
        //                currentY -= yTick;
        //            }
        //        }
        //        distanceTravelled = Math.Sqrt(Math.Pow(Math.Abs(_x - currentX), 2) + Math.Pow(Math.Abs(_y - currentY), 2));
        //        double timeOfTravel = distanceTravelled / velocity;
        //        await Task.Delay(TimeSpan.FromSeconds(timeOfTravel));
        //        lock (_moveLock)
        //        {
        //            //Console.WriteLine($"Ball moved to {currentX}, {currentY}");
        //            _x = currentX;
        //            _y = currentY;
        //            NotifyObservers(this);
        //        }
        //    }
        //}

        public override IDisposable Subscribe(IObserver<Ball> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new SubscriptionToken(_observers, observer);
        }

        public void NotifyObservers(Ball ball)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(ball);
            }
        }
    }

    public class SubscriptionToken : IDisposable
    {
        private List<IObserver<Ball>> _observers;
        private IObserver<Ball> _observer;

        public SubscriptionToken(List<IObserver<Ball>> observers, IObserver<Ball> observer)
        {
            this._observers = observers;
            this._observer = observer;
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
