using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace Model
{
    internal class Model : ModelAPI, IObserver<LogicAPI>
    {
        private readonly LogicAPI table;
        private readonly ObservableCollection<BallModelAPI> _balls = new ObservableCollection<BallModelAPI>();
        private readonly object _ballsLock = new object();
        private IDisposable? _subscriptionToken;

        public override int Width => table.Width;

        public override int Height => table.Height;

        public Model()
        {
            this.table = LogicAPI.Instance(690, 420);
            this.Subscribe(table);
        }

        public Model(LogicAPI table)
        {
            this.table = table;
        }

        public override void CreateBalls(int number, int radius)
        {
            table.CreateBalls(number, radius);
            List<List<float>> ballPositions = table.GetBallPositions();
            lock (_ballsLock)
            {
                for (int i = 0; i < ballPositions.Count; i++)
                {
                    BallModel ball = new BallModel(ballPositions[i][0], ballPositions[i][1], radius);
                    _balls.Add(ball);
                }
            }
        }

        public override void Start(float velocity)
        {
            table.Start(velocity);
        }

        public override void ResetTable()
        {
            lock (_ballsLock)
            {
                _balls.Clear();
            }
            table.ResetTable();
        }

        public override ObservableCollection<BallModelAPI> Balls => _balls;

        public void Subscribe(IObservable<LogicAPI> provider)
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

        public void OnNext(LogicAPI value)
        {
            lock (_ballsLock)
            {
                List<List<float>> ballPositions = table.GetBallPositions();
                if (_balls.Count == ballPositions.Count)
                {
                    for (int i = 0; i < ballPositions.Count; i++)
                    {
                        if (_balls[i].X != ballPositions[i][0])
                        {
                            _balls[i].X = ballPositions[i][0];
                        }
                        if (_balls[i].Y != ballPositions[i][1])
                        {
                            _balls[i].Y = ballPositions[i][1];
                        }
                    }
                }
            }
        }
    }
}
