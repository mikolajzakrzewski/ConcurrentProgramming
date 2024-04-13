using Model;

namespace ModelTest
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void CreateAPITest() 
        {
            ModelAPI modelAPI = ModelAPI.Instance();
            Assert.IsNotNull(modelAPI);
        }

        [TestMethod]
        public void TestCreateBalls()
        {
            ModelAPI model;
            model = ModelAPI.Instance();
            int numberOfBalls = 5;
            int radius = 10;

            model.CreateBalls(numberOfBalls, radius);

            Assert.AreEqual(numberOfBalls, model.Balls.Count);
            foreach (var ball in model.Balls)
            {
                Assert.AreEqual(radius, ball.Radius);
            }
        }

        [TestMethod]
        public void TestResetTable() 
        {
            ModelAPI model;
            model = ModelAPI.Instance();

            model.CreateBalls(5, 10);

            model.ResetTable();

            Assert.AreEqual(0, model.Balls.Count);
        }
    }
}