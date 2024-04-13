using Data;

namespace DataTest
{
    //[TestClass]
    //public class BallTest
    //{
    //[TestMethod]
    //public void MoveTest()
    //{
    //    DataAPI ball = DataAPI.Instance(10, 10, 20);
    //    Task.Run(async () =>
    //    {
    //        await ball.Move(20, 30, 5);
    //    }).Wait();
    //    Assert.AreEqual(20, ball.X);
    //    Assert.AreEqual(30, ball.Y);
    //}
    //}

    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void CreateAPITest()
        {
            DataAPI dataAPI = DataAPI.Instance(10, 10, 10);
            Assert.IsNotNull(dataAPI);
        }
    }
}