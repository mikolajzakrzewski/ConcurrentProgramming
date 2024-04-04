using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Table : LogicAPI
    {
        private readonly int _width;
        private readonly int _height;
        private readonly List<Ball> _balls;

        public Table(int width, int height)
        {
            this._width = width;
            this._height = height;
            this._balls = new List<Ball>();
        }

        public int Width
        {
            get => _width;
        }

        public int Height
        {
            get => _height;
        }

        public List<Ball> Balls
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
                Ball ball = new Ball(x, y, radius);
                _balls.Add(ball);
            }
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override List<List<float>> GetBallPositions()
        {
            List<List<float>> positions = new List<List<float>>();
            for (int i = 0; i < _balls.Count; i++)
            {
                List<float> position = new List<float>();
                position.Add(_balls[i].X);
                position.Add(_balls[i].Y);
            }
            return positions;
        }

        public override void ResetTable()
        {
            _balls.Clear();
        }
    }
}
