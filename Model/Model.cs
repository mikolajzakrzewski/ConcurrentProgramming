using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Model : ModelAPI
    {
        private readonly LogicAPI table = LogicAPI.Instance(690, 420);
        private readonly ObservableCollection<BallModel> _balls = new ObservableCollection<BallModel>();

        public override int Width => table.Width;

        public override int Height => table.Height;

        public override void CreateBalls(int number, int radius)
        {
            table.CreateBalls(number, radius);
            for (int i = 0; i < table.Balls.Count; i++)
            {
                BallModel ball = new BallModel(table.Balls[i].X, table.Balls[i].Y, table.Balls[i].Radius);
                _balls.Add(ball);
                // TODO: replace IObserver<DataAPI> implementation
                table.Balls[i].Subscribe(ball);
                // end
            }
        }

        public override void Start(double velocity)
        {
            table.Start(velocity);
        }

        public override void ResetTable()
        {
            table.ResetTable();
            _balls.Clear();
        }

        public override ObservableCollection<BallModel> Balls => _balls;
    }
}
