using System.Numerics;

namespace Data;

internal class BallDto
{
    public BallDto(int id, Vector2 position, Vector2 velocity, DateTime date)
    {
        BallId = id;
        PositionX = position.X;
        PositionY = position.Y;
        VelocityX = velocity.X;
        VelocityY = velocity.Y;
        Date = date;
    }

    public int BallId { get; }
    public float PositionX { get; }
    public float PositionY { get; }
    public float VelocityX { get; }
    public float VelocityY { get; }
    public DateTime Date { get; }
}