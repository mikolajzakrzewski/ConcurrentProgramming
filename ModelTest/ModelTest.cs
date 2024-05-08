using Logic;
using Model;

namespace ModelTest;

internal class FakeLogicApi : LogicApi
{
    public override int Width => throw new NotImplementedException();

    public override int Height => throw new NotImplementedException();

    public override void CreateBalls(int number, int radius)
    {
    }

    public override List<List<float>> GetBallPositions()
    {
        var ballPositions = new List<List<float>>();
        for (var i = 0; i < 5; i++)
        {
            var ballPosition = new List<float> { 3, 3 };
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
        throw new NotImplementedException();
    }
}

[TestClass]
public class ModelTest
{
    [TestMethod]
    public void CreateApiTest()
    {
        var table = new FakeLogicApi();
        var modelApi = ModelApi.Instance(table);
        Assert.IsNotNull(modelApi);
    }

    [TestMethod]
    public void TestCreateBalls()
    {
        var table = new FakeLogicApi();
        var model = ModelApi.Instance(table);
        const int numberOfBalls = 5;
        const int radius = 10;
        model.CreateBalls(numberOfBalls, radius);
        Assert.AreEqual(numberOfBalls, model.Balls.Count);
        foreach (var ball in model.Balls) Assert.AreEqual(radius, ball.Radius);
    }

    [TestMethod]
    public void TestResetTable()
    {
        var table = new FakeLogicApi();
        var model = ModelApi.Instance(table);
        model.CreateBalls(5, 10);
        model.ResetTable();
        Assert.AreEqual(0, model.Balls.Count);
    }
}