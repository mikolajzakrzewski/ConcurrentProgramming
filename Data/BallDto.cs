using System.Numerics;

namespace Data
{
    internal class BallDto
    {
        public float PositionX { get; }
        public float PositionY { get; }
        public float VelocityX { get; }
        public float VelocityY { get; }
        public string date {  get; }

        public BallDto(Vector2 position, Vector2 velocity, string date)
        {
            PositionX = position.X;
            PositionY = position.Y;
            VelocityX = velocity.X;
            VelocityY = velocity.Y;
            this.date = date;
        }
    }
}
