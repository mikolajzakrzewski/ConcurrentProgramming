using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        private readonly object _ballsLock = new object();
        private IDisposable? _subscriptionToken;
        private List<IObserver<LogicAPI>> _observers;

        public Table(int width, int height)
        {
            this._width = width;
            this._height = height;
            lock (_ballsLock)
            {
                this._balls = new List<DataAPI>();
            }
            this._observers = new List<IObserver<LogicAPI>>();
        }

        public Table(int width, int height, List<DataAPI> balls)
        {
            this._width = width;
            this._height = height;
            lock (_ballsLock)
            {
                this._balls = balls;
            }
        }

        public override int Width
        {
            get => _width;
        }

        public override int Height
        {
            get => _height;
        }

        public List<DataAPI> Balls
        {
            get => _balls;
        }

        public override List<List<float>> GetBallPositions()
        {
            List<List<float>> ballPositions = new List<List<float>>();
            lock (_ballsLock)
            {
                foreach (var ball in _balls)
                {
                    List<float> ballPosition = new List<float> { ball.Position.X, ball.Position.Y };
                    ballPositions.Add(ballPosition);
                }
            }
            return ballPositions;
        }

        public override void CreateBalls(int number, int radius)
        {
            lock (_ballsLock)
            {
                for (int i = 0; i < number; i++)
                {
                    var rand = new Random();
                    float x = rand.Next(0 + radius, _width - radius);
                    float y = rand.Next(0 + radius, _height - radius);
                    DataAPI ball = DataAPI.Instance(new System.Numerics.Vector2(x, y), radius, 200);
                    _balls.Add(ball);
                    this.Subscribe(ball);
                }
            }
        }

        public override void Start(float velocity)
        {
            lock (_ballsLock)
            {
                foreach (var ball in _balls)
                {
                    ball.Move(velocity);
                }
            }
        }

        public override void ResetTable()
        {
            lock (_ballsLock)
            {
                _balls.Clear();
            }
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

            lock (_ballsLock)
            {
                foreach (var ball1 in _balls)
                {
                    foreach (var ball2 in _balls)
                    {
                        if (ball1 != ball2)
                        {
                            BallCollision(ball1, ball2);
                        }
                    }
                }
            }

            NotifyObservers(this);
        }

        private void WallCollision(DataAPI ball)
        {
            if (ball.Position.X + ball.Radius > _width || ball.Position.X - ball.Radius < 0 || ball.Position.Y + ball.Radius > _height || ball.Position.Y - ball.Radius < 0)
            {
                bool hitTopOrBottom = false;

                if (ball.Position.Y - ball.Radius < 0 || ball.Position.Y + ball.Radius > _height)
                {
                    hitTopOrBottom = true;
                }

                if (hitTopOrBottom)
                {
                    float exceededDistance;
                    if (ball.Position.Y - ball.Radius < 0)
                    {
                        exceededDistance = Math.Abs(ball.Position.Y - ball.Radius);
                        ball.Position = new Vector2(ball.Position.X, ball.Radius);
                    }
                    else
                    {
                        exceededDistance = Math.Abs(ball.Position.Y + ball.Radius - _height);
                        ball.Position = new Vector2(ball.Position.X, _height - ball.Radius);
                    }
                    ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                    float velocity = (float)Math.Sqrt(ball.Velocity.X * ball.Velocity.X + ball.Velocity.Y * ball.Velocity.Y);
                    float timeOfExceededTravel = exceededDistance / velocity;
                    ball.Position -= ball.Velocity * timeOfExceededTravel;
                }
                else
                {
                    float exceededDistance;
                    if (ball.Position.X - ball.Radius < 0)
                    {
                        exceededDistance = Math.Abs(ball.Position.X - ball.Radius);
                        ball.Position = new Vector2(ball.Radius, ball.Position.Y);
                    }
                    else
                    {
                        exceededDistance = Math.Abs(ball.Position.X + ball.Radius - _width);
                        ball.Position = new Vector2(_width - ball.Radius, ball.Position.Y);
                    }
                    ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                    float velocity = (float)Math.Sqrt(ball.Velocity.X * ball.Velocity.X + ball.Velocity.Y * ball.Velocity.Y);
                    float timeOfExceededTravel = exceededDistance / velocity;
                    ball.Position -= ball.Velocity * timeOfExceededTravel;
                }
            }
        }

        private void BallCollision(DataAPI ball1, DataAPI ball2)
        {
            float dx = ball2.Position.X - ball1.Position.X;
            float dy = ball2.Position.Y - ball1.Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            if (distance < ball1.Radius + ball2.Radius)
            {
                float collisionAngle = (float)Math.Atan2(dy, dx);

                float overlap = (ball1.Radius + ball2.Radius) - distance;
                float mtdX = overlap * (float)Math.Cos(collisionAngle);
                float mtdY = overlap * (float)Math.Sin(collisionAngle);

                ball1.Position -= new System.Numerics.Vector2(mtdX / 2, mtdY / 2);
                ball2.Position += new System.Numerics.Vector2(mtdX / 2, mtdY / 2);

                float sepX = -dy;
                float sepY = dx;
                float sepLength = (float)Math.Sqrt(sepX * sepX + sepY * sepY);
                sepX /= sepLength;
                sepY /= sepLength;

                ball1.Position += new System.Numerics.Vector2(sepX * 0.5f, sepY * 0.5f);
                ball2.Position -= new System.Numerics.Vector2(sepX * 0.5f, sepY * 0.5f);

                ReflectVelocities(ball1, ball2, collisionAngle);
            }
        }

        private void ReflectVelocities(DataAPI ball1, DataAPI ball2, float collisionAngle)
        {
            float combinedMass = ball1.Mass + ball2.Mass;
            float newVelX1 = ((ball1.Velocity.X * (ball1.Mass - ball2.Mass)) + (2 * ball2.Mass * ball2.Velocity.X)) / combinedMass;
            float newVelY1 = ((ball1.Velocity.Y * (ball1.Mass - ball2.Mass)) + (2 * ball2.Mass * ball2.Velocity.Y)) / combinedMass;
            float newVelX2 = ((ball2.Velocity.X * (ball2.Mass - ball1.Mass)) + (2 * ball1.Mass * ball1.Velocity.X)) / combinedMass;
            float newVelY2 = ((ball2.Velocity.Y * (ball2.Mass - ball1.Mass)) + (2 * ball1.Mass * ball1.Velocity.Y)) / combinedMass;

            ball1.Velocity = new System.Numerics.Vector2(newVelX1, newVelY1);
            ball2.Velocity = new System.Numerics.Vector2(newVelX2, newVelY2);
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
