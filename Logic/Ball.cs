namespace Logic
{
    public class Ball
    {
        private int _x;
        private int _y;
        private int _radius;

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
            set => _radius = value;
        }
    }
}