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
        private readonly LogicAPI table = LogicAPI.Instance(690, 420);
        private readonly ObservableCollection<BallModel> _balls = new ObservableCollection<BallModel>();
        private IDisposable? _subscriptionToken;

        public override int Width => table.Width;

        public override int Height => table.Height;

        public Model()
        {
            this.Subscribe(table);
        }

        public override void CreateBalls(int number, int radius)
        {
            table.CreateBalls(number, radius);
            for (int i = 0; i < table.Balls.Count; i++)
            {
                BallModel ball = new BallModel(table.Balls[i].X, table.Balls[i].Y, table.Balls[i].Radius);
                _balls.Add(ball);
            }
        }

        public override void Start(float velocity)
        {
            table.Start(velocity);
        }

        public override void ResetTable()
        {
            table.ResetTable();
            _balls.Clear();
        }

        public override ObservableCollection<BallModel> Balls => _balls;

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
            for (int i = 0; i < _balls.Count; i++)
            {
                if (_balls[i].X != table.Balls[i].X)
                {
                    _balls[i].X = table.Balls[i].X;
                }
                if (_balls[i].Y != table.Balls[i].Y)
                {
                    _balls[i].Y = table.Balls[i].Y;
                }
            }
        }
    }
}
