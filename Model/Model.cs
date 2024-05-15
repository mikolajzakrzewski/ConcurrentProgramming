using System.Collections.ObjectModel;
using System.Numerics;
using Logic;

namespace Model;

internal class Model : ModelApi, IObserver<LogicApi>
{
    private readonly ObservableCollection<BallModelApi> _balls = [];
    private readonly object _ballsLock = new();
    private readonly LogicApi _table;
    private IDisposable? _subscriptionToken;

    public Model()
    {
        _table = LogicApi.Instance(690, 420);
        Subscribe(_table);
    }

    public Model(LogicApi table)
    {
        _table = table;
    }

    public override int Width => _table.Width;

    public override int Height => _table.Height;

    public override ObservableCollection<BallModelApi> Balls
    {
        get
        {
            lock (_ballsLock)
            {
                return _balls;
            }
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

    public void OnNext(LogicApi value)
    {
        lock (_ballsLock)
        {
            var ballPositions = _table.GetBallPositions();
            if (_balls.Count != ballPositions.Count) return;
            for (var i = 0; i < ballPositions.Count; i++)
            {
                if (_balls[i].X != ballPositions[i][0]) _balls[i].X = ballPositions[i][0];
                if (_balls[i].Y != ballPositions[i][1]) _balls[i].Y = ballPositions[i][1];
            }
        }
    }

    public override void Start(int number, int radius, float velocity)
    {
        _table.Start(number, radius, velocity);
        var ballPositions = _table.GetBallPositions();
        lock (_ballsLock)
        {
            foreach (var ballPosition in ballPositions)
            {
                var ball = new BallModel(new Vector2(ballPosition[0], ballPosition[1]), radius);
                _balls.Add(ball);
            }
        }
    }

    public override void ResetTable()
    {
        lock (_ballsLock)
        {
            _balls.Clear();
        }

        _table.ResetTable();
    }

    public void Subscribe(IObservable<LogicApi> provider)
    {
        if (provider != null) _subscriptionToken = provider.Subscribe(this);
    }

    public void Unsubscribe()
    {
        _subscriptionToken?.Dispose();
    }
}