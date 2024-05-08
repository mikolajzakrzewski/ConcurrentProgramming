using Logic;
using Model;

namespace ModelTest
{
    internal class FakeLogicAPI : LogicApi
    {
        public override int Width => throw new System.NotImplementedException();

        public override int Height => throw new System.NotImplementedException();

        public override void CreateBalls(int number, int radius)
        {
            
        }

        public override List<List<float>> GetBallPositions()
        {
            List<List<float>> ballPositions = new List<List<float>>();
            for (int i = 0; i < 5; i++)
            {
                List<float> ballPosition = new List<float> { 3, 3 };
                ballPositions.Add(ballPosition);
            }
            return ballPositions;
        }

        public override void ResetTable()
        {
            
        }

        public override void Start(float velocity)
        {
            
        }

        public override IDisposable Subscribe(IObserver<LogicApi> observer)
        {
            throw new System.NotImplementedException();
        }
    }

    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void CreateAPITest() 
        {
            FakeLogicAPI table = new FakeLogicAPI();
            ModelAPI modelAPI = ModelAPI.Instance(table);
            Assert.IsNotNull(modelAPI);
        }

        [TestMethod]
        public void TestCreateBalls()
        {
            ModelAPI model;
            FakeLogicAPI table = new FakeLogicAPI();
            model = ModelAPI.Instance(table);
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
            FakeLogicAPI table = new FakeLogicAPI();
            model = ModelAPI.Instance(table);
            model.CreateBalls(5, 10);
            model.ResetTable();
            Assert.AreEqual(0, model.Balls.Count);
        }
    }
}