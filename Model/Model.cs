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
        private readonly Table table = LogicAPI.Instance();
        private readonly ObservableCollection<BallModel> balls;

        public override int Width => table.Width;

        public override int Height => table.Height;

        public override void CreateBalls(int number, int radius)
        {
            table.CreateBalls(number, radius);
            for (int i = 0; i < table.Balls.Count; i++)
            {
                BallModel ball = new BallModel(table.Balls[i].X, table.Balls[i].Y, table.Balls[i].Radius);
                balls.Add(ball);
            }
        }

        public override void Start()
        {
            table.Start();
        }

        public override void ResetTable()
        {
            table.ResetTable();
            balls.Clear();
        }
    }
}
