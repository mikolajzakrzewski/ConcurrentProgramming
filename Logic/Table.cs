using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Data;

namespace Logic
{
    internal class Table : LogicAPI, IObserver<DataAPI>, IObservable<LogicAPI>
    {
        private readonly int _width;
        private readonly int _height;
        private readonly List<DataAPI> _balls;
        private IDisposable? _subscriptionToken;
        private List<IObserver<LogicAPI>> _observers;

        public Table(int width, int height)
        {
            this._width = width;
            this._height = height;
            this._balls = new List<DataAPI>();
            this._observers = new List<IObserver<LogicAPI>>();
        }

        public override int Width
        {
            get => _width;
        }

        public override int Height
        {
            get => _height;
        }

        public override List<DataAPI> Balls
        {
            get => _balls;
        }

        public override void CreateBalls(int number, int radius)
        {
            for (int i = 0; i < number; i++)
            {
                var rand = new Random();
                float x = rand.Next(0 + radius, _width - radius);
                float y = rand.Next(0 + radius, _height - radius);
                DataAPI ball = DataAPI.Instance(x, y, radius);
                _balls.Add(ball);
                this.Subscribe(ball);
            }
        }

        public override void Start(float velocity)
        {
            var rand = new Random();
            foreach (var ball in _balls)
            {
                float newX = rand.Next(0 + ball.Radius, _width - ball.Radius);
                float newY = rand.Next(0 + ball.Radius, _height - ball.Radius);
                Task.Run(() => { ball.Move(velocity); });
            }
        }

        public override void ResetTable()
        {
            _balls.Clear();
        }

        public void Subscribe(IObservable<DataAPI> provider)
        {
            if (provider != null)
            {
                _subscriptionToken = provider.Subscribe(this);
            }
        }

        public void Unsubscribe()
        {
            if (_subscriptionToken != null)
            {
                _subscriptionToken.Dispose();
            }
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DataAPI value)
        {
            WallCollision(value);
            NotifyObservers(this);
        }

        private void WallCollision(DataAPI ball)
        {
            if (ball.X + ball.Radius > _width || ball.X - ball.Radius < 0 || ball.Y + ball.Radius > _height || ball.Y - ball.Radius < 0)
            {
                bool hitTopOrBottom = false;

                if (ball.Y - ball.Radius < 0 || ball.Y + ball.Radius > _height)
                {
                    hitTopOrBottom = true;
                }

                if (hitTopOrBottom)
                {
                    float exceededDistance;
                    if (ball.Y - ball.Radius < 0)
                    {
                        exceededDistance = (float)Math.Abs(ball.Y - ball.Radius);
                    }
                    else
                    {
                        exceededDistance = (float)Math.Abs(ball.Y + ball.Radius - _height);
                    }
                    ball.VelocityY *= -1;
                    float velocity = (float)Math.Sqrt((float)Math.Pow(ball.VelocityX, 2) + (float)Math.Pow(ball.VelocityY, 2));
                    float timeOfExceededTravel = exceededDistance / velocity;
                    ball.X -= ball.VelocityX * timeOfExceededTravel;
                    ball.Y += ball.VelocityY * timeOfExceededTravel;
                }
                else
                {
                    float exceededDistance;
                    if (ball.X - ball.Radius < 0)
                    {
                        exceededDistance = (float)Math.Abs(ball.X - ball.Radius);
                    }
                    else
                    {
                        exceededDistance = (float)Math.Abs(ball.X + ball.Radius - _width);
                    }
                    ball.VelocityX *= -1;
                    float velocity = (float)Math.Sqrt((float)Math.Pow(ball.VelocityX, 2) + (float)Math.Pow(ball.VelocityY, 2));
                    float timeOfExceededTravel = exceededDistance / velocity;
                    ball.Y -= ball.VelocityY * timeOfExceededTravel;
                    ball.X += ball.VelocityX * timeOfExceededTravel;
                }
            }
        }

        public override IDisposable Subscribe(IObserver<LogicAPI> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new SubscriptionToken(_observers, observer);
        }

        public void NotifyObservers(LogicAPI table)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(table);
            }
        }
    }

    public class SubscriptionToken : IDisposable
    {
        private List<IObserver<LogicAPI>> _observers;
        private IObserver<LogicAPI> _observer;

        public SubscriptionToken(List<IObserver<LogicAPI>> observers, IObserver<LogicAPI> observer)
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
