namespace Logic
{
    public class Ball
    {
        int positionX;
        int positionY;
        int radius;
        double velocityX;
        double velocityY;

        public Ball(int x, int y, int radius, double startVelocityX, double startVelocityY)
        {
            this.positionX = x;
            this.positionY = y;
            this.radius = radius;
            this.velocityX = startVelocityX;
            this.velocityY = startVelocityY;
        }
    }
}