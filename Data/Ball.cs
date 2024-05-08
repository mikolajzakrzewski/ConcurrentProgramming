using System.Diagnostics;
using System.Numerics;

namespace Data
{
    internal class Ball : DataAPI, IObservable<DataAPI>
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private readonly int _radius;
        private readonly int _mass;
        private readonly object _moveLock = new object();
        private readonly object _velocityLock = new object();
        private List<IObserver<DataAPI>> _observers;

        public Ball(Vector2 position, int radius, int mass)
        {
            this._position = position;
            this._radius = radius;
            this._observers = new List<IObserver<DataAPI>>();
            this._mass = mass;
        }

        public override Vector2 Position
        {
            get => _position;
            set
            {
                lock (_moveLock)
                {
                    _position = value;
                }
            }
        }

        public override Vector2 Velocity
        {
            get => _velocity;
            set
            {
                lock (_velocityLock)
                {
                    _velocity = value;
                }
            }
        }

        public override int Radius
        {
            get => _radius;
        }

        public override int Mass
        {
            get => _mass;
        }

        public override async Task Move(float velocity)
        {
            Random rand = new();
            float moveAngle = rand.Next(0, 360);
            Velocity = new Vector2(velocity * (float)Math.Cos(moveAngle), velocity * (float)Math.Sin(moveAngle));
            const float timeOfTravel = 1f / 60f;
            while (true)
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                await Task.Delay(TimeSpan.FromSeconds(timeOfTravel));
                stopwatch.Stop();
                var timeElapsed = (float)stopwatch.Elapsed.TotalSeconds;
                var velocityChange = Velocity * (timeElapsed);
                lock (_moveLock)
                {
                    _position += velocityChange;
                }
                NotifyObservers(this);
            }
        }

        public override IDisposable Subscribe(IObserver<DataAPI> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new SubscriptionToken(_observers, observer);
        }

        public void NotifyObservers(DataAPI ball)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(ball);
            }
        }
    }

    public class SubscriptionToken : IDisposable
    {
        private List<IObserver<DataAPI>> _observers;
        private IObserver<DataAPI> _observer;

        public SubscriptionToken(List<IObserver<DataAPI>> observers, IObserver<DataAPI> observer)
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
