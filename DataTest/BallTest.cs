using Data;

namespace LogicTest
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void MoveTest()
        {
            Ball ball = new Ball(10, 10, 20);
            Task.Run(async () =>
            {
                await ball.Move(20, 30, 5);
            }).Wait();
            Assert.AreEqual(20, ball.X);
            Assert.AreEqual(30, ball.Y);
        }
    }
}