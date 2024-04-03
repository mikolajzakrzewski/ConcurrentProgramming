namespace Logic
{
    public class Ball
    {
        private int _x;
        private int _y;
        private readonly int _radius;

        public Ball(int x, int y, int radius)
        {
            this._x = x;
            this._x = y;
            this._radius = radius;
        }

        public int X
        {
            get => _x;
            set => _x = value;
        }

        public int Y
        {
            get => _y;
            set => _y = value;
        }

        public int Radius
        {
            get => _radius;
        }

        public async Task Move(int x, int y, double velocity)
        {
            int xDiff = Math.Abs(_x - x);
            int yDiff = Math.Abs(_y - y);
            double distanceTravelled = Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
            await Task.Delay(TimeSpan.FromSeconds(distanceTravelled / velocity));
            _x = x;
            _y = y;
        }
    }
}