using Logic;
using Model;

namespace ModelTest;

internal class FakeLogicApi : LogicApi
{
    public override int Width => throw new NotImplementedException();

    public override int Height => throw new NotImplementedException();

    public override List<List<float>> GetBallPositions()
    {
        var ballPositions = new List<List<float>>();
        for (var i = 0; i < 1; i++)
        {
            var ballPosition = new List<float> { 1, 1 };
            ballPositions.Add(ballPosition);
        }

        return ballPositions;
    }

    public override void ResetTable()
    {
    }

    public override void Start(int radius, int number, float velocity)
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
    public void StartTest()
    {
        var table = new FakeLogicApi();
        var model = ModelApi.Instance(table);
        const int numberOfBalls = 1;
        const int radius = 1;
        model.Start(numberOfBalls, radius, 1);
        Assert.AreEqual(numberOfBalls, model.Balls.Count);
        foreach (var ball in model.Balls) Assert.AreEqual(radius, ball.Radius);
    }

    [TestMethod]
    public void ResetTableTest()
    {
        var table = new FakeLogicApi();
        var model = ModelApi.Instance(table);
        model.Start(1, 1, 1);
        model.ResetTable();
        Assert.AreEqual(0, model.Balls.Count);
    }
}