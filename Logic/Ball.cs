namespace Logic
{
    public class Ball
    {
        private float _x;
        private float _y;
        private readonly int _radius;
        private readonly object _moveLock = new object();

        public Ball(float x, float y, int radius)
        {
            this._x = x;
            this._y = y;
            this._radius = radius;
        }

        public float X
        {
            get => _x;
            set => _x = value;
        }

        public float Y
        {
            get => _y;
            set => _y = value;
        }

        public int Radius
        {
            get => _radius;
        }

        public async Task Move(float x, float y, double velocity)
        {
            float xDiff = Math.Abs(_x - x);
            float yDiff = Math.Abs(_y - y);
            float xTick = 1;
            float yTick = 1;
            if (xDiff > yDiff)
            {
                yTick = yDiff / xDiff;
            }
            else if (yDiff > xDiff)
            {
                xTick = xDiff / yDiff;
            }
            double distanceTravelled;
            float currentX = _x;
            float currentY = _y;
            while (currentX != x || currentY != y)
            {
                if (currentX < x)
                {
                    if (currentX + xTick > x)
                    {
                        currentX = x;
                    }
                    else
                    {
                        currentX += xTick;
                    }
                }
                else if (currentX > x)
                {
                    if (currentX - xTick < x)
                    {
                        currentX = x;
                    }
                    else
                    {
                        currentX -= xTick;
                    }
                }
                if (currentY < y)
                {
                    if (currentY + yTick > y)
                    {
                        currentY = y;
                    }
                    else
                    {
                        currentY += yTick;
                    }
                }
                else if (currentY > y)
                {
                    if (currentY - yTick < y)
                    {
                        currentY = y;
                    }
                    else
                    {
                        currentY -= yTick;
                    }
                }
                distanceTravelled = Math.Sqrt(Math.Pow(Math.Abs(_x - currentX), 2) + Math.Pow(Math.Abs(_y - currentY), 2));
                double timeOfTravel = distanceTravelled / velocity;
                await Task.Delay(TimeSpan.FromSeconds(timeOfTravel));
                lock(_moveLock) {
                    Console.WriteLine($"Ball moved to {currentX}, {currentY}");
                    _x = currentX;
                    _y = currentY;
                }
            }
        }
    }
}