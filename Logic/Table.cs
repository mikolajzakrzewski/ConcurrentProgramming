using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class Table
    {
        private readonly int _width;
        private readonly int _height;
        private List<Ball> _balls;

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
            set => _balls = value;
        }

        public void AddBall(Ball ball)
        {
            _balls.Add(ball);
        }

        public void RemoveBall(Ball ball)
        {
            _balls.Remove(ball);
        }
    }
}
