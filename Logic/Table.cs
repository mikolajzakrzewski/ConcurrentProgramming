using System.Numerics;
using Data;

namespace Logic;

internal class Table : LogicApi, IObserver<DataApi>, IObservable<LogicApi>
{
    private readonly object _ballsLock = new();
    private readonly int _height;
    private readonly int _width;
    private readonly List<IObserver<LogicApi>>? _observers;
    private IDisposable? _subscriptionToken;

    public Table(int width, int height)
    {
        _width = width;
        _height = height;
        lock (_ballsLock)
        {
            Balls = [];
        }

        _observers = [];
    }

    public Table(int width, int height, List<DataApi> balls)
    {
        _width = width;
        _height = height;
        lock (_ballsLock)
        {
            Balls = balls;
        }
    }

    public override int Width => _width;

    public override int Height => _height;

    public List<DataApi> Balls { get; }

    public override IDisposable Subscribe(IObserver<LogicApi> observer)
    {
        if (!_observers.Contains(observer)) _observers.Add(observer);
        return new SubscriptionToken(_observers, observer);
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(DataApi value)
    {
        WallCollision(value);

        lock (_ballsLock)
        {
            foreach (var ball1 in Balls)
                foreach (var ball2 in Balls)
                    if (ball1 != ball2)
                        BallCollision(ball1, ball2);
        }

        NotifyObservers(this);
    }

    public override List<List<float>> GetBallPositions()
    {
        var ballPositions = new List<List<float>>();
        lock (_ballsLock)
        {
            foreach (var ball in Balls)
            {
                var ballPosition = new List<float> { ball.Position.X, ball.Position.Y };
                ballPositions.Add(ballPosition);
            }
        }

        return ballPositions;
    }

    public override void CreateBalls(int number, int radius)
    {
        lock (_ballsLock)
        {
            for (var i = 0; i < number; i++)
            {
                var rand = new Random();
                float x = rand.Next(0 + radius, _width - radius);
                float y = rand.Next(0 + radius, _height - radius);
                var ball = DataApi.Instance(new Vector2(x, y), radius, 200);
                Balls.Add(ball);
                Subscribe(ball);
            }
        }
    }

    public override void Start(float velocity)
    {
        lock (_ballsLock)
        {
            foreach (var ball in Balls)
                Task.Run(() => { ball.Move(velocity); });
        }
    }

    public override void ResetTable()
    {
        lock (_ballsLock)
        {
            Balls.Clear();
        }
    }

    public void Subscribe(IObservable<DataApi> provider)
    {
        if (provider != null) _subscriptionToken = provider.Subscribe(this);
    }

    public void Unsubscribe()
    {
        _subscriptionToken?.Dispose();
    }

    private void WallCollision(DataApi ball)
    {
        if (!(ball.Position.X + ball.Radius > _width) && !(ball.Position.X - ball.Radius < 0) &&
            !(ball.Position.Y + ball.Radius > _height) && !(ball.Position.Y - ball.Radius < 0)) return;
        var hitTopOrBottom = ball.Position.Y - ball.Radius < 0 || ball.Position.Y + ball.Radius > _height;
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
            var velocity = (float)Math.Sqrt(ball.Velocity.X * ball.Velocity.X + ball.Velocity.Y * ball.Velocity.Y);
            var timeOfExceededTravel = exceededDistance / velocity;
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
            var velocity = (float)Math.Sqrt(ball.Velocity.X * ball.Velocity.X + ball.Velocity.Y * ball.Velocity.Y);
            var timeOfExceededTravel = exceededDistance / velocity;
            ball.Position -= ball.Velocity * timeOfExceededTravel;
        }
    }

    private static void BallCollision(DataApi ball1, DataApi ball2)
    {
        var dx = ball2.Position.X - ball1.Position.X;
        var dy = ball2.Position.Y - ball1.Position.Y;
        var distance = (float)Math.Sqrt(dx * dx + dy * dy);

        if (!(distance < ball1.Radius + ball2.Radius)) return;
        var collisionAngle = (float)Math.Atan2(dy, dx);

        var overlap = ball1.Radius + ball2.Radius - distance;
        var mtdX = overlap * (float)Math.Cos(collisionAngle);
        var mtdY = overlap * (float)Math.Sin(collisionAngle);

        ball1.Position -= new Vector2(mtdX / 2, mtdY / 2);
        ball2.Position += new Vector2(mtdX / 2, mtdY / 2);

        var sepX = -dy;
        var sepY = dx;
        var sepLength = (float)Math.Sqrt(sepX * sepX + sepY * sepY);
        sepX /= sepLength;
        sepY /= sepLength;

        ball1.Position += new Vector2(sepX * 0.5f, sepY * 0.5f);
        ball2.Position -= new Vector2(sepX * 0.5f, sepY * 0.5f);

        float combinedMass = ball1.Mass + ball2.Mass;
        var newVelX1 = (ball1.Velocity.X * (ball1.Mass - ball2.Mass) + 2 * ball2.Mass * ball2.Velocity.X) /
                       combinedMass;
        var newVelY1 = (ball1.Velocity.Y * (ball1.Mass - ball2.Mass) + 2 * ball2.Mass * ball2.Velocity.Y) /
                       combinedMass;
        var newVelX2 = (ball2.Velocity.X * (ball2.Mass - ball1.Mass) + 2 * ball1.Mass * ball1.Velocity.X) /
                       combinedMass;
        var newVelY2 = (ball2.Velocity.Y * (ball2.Mass - ball1.Mass) + 2 * ball1.Mass * ball1.Velocity.Y) /
                       combinedMass;

        ball1.Velocity = new Vector2(newVelX1, newVelY1);
        ball2.Velocity = new Vector2(newVelX2, newVelY2);
    }

    public void NotifyObservers(LogicApi table)
    {
        foreach (var observer in _observers) observer.OnNext(table);
    }
}

public class SubscriptionToken(ICollection<IObserver<LogicApi>> observers, IObserver<LogicApi> observer)
    : IDisposable
{
    public void Dispose()
    {
        if (observer != null && observers.Contains(observer)) observers.Remove(observer);
    }
}