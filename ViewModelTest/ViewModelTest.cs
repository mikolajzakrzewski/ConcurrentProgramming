using System.Collections.ObjectModel;
using Model;
using ViewModel;

namespace ViewModelTest;

internal class FakeModelApi : ModelApi
{
    public override int Width => 2;

    public override int Height => 2;

    public override ObservableCollection<BallModelApi> Balls => throw new NotImplementedException();

    public override void CreateBalls(int number, int radius)
    {
    }

    public override void Start(float velocity)
    {
    }

    public override void ResetTable()
    {
    }
}

internal class FakeBallModelApi : BallModelApi
{
    public override float X
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public override float Y
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public override int Radius => throw new NotImplementedException();
}

[TestClass]
public class ViewModelTest
{
    [TestMethod]
    public void TestCanCreateBalls()
    {
        var viewModel = new MainWindowViewModel(new FakeModelApi());
        Assert.IsFalse(viewModel.CanCreateBalls());
        viewModel.BallsAmount = 2;
        viewModel.Radius = 2;
        Assert.IsTrue(viewModel.CanCreateBalls());
        BallModelApi fakeBallModelApi = new FakeBallModelApi();
        viewModel.Balls.Add(fakeBallModelApi);
        Assert.IsFalse(viewModel.CanCreateBalls());
    }

    [TestMethod]
    public void TestCanStart()
    {
        var viewModel = new MainWindowViewModel(new FakeModelApi());
        BallModelApi fakeBallModelApi = new FakeBallModelApi();
        viewModel.Balls.Add(fakeBallModelApi);
        viewModel.Velocity = 5;
        Assert.IsTrue(viewModel.CanStart());
    }

    [TestMethod]
    public void TestCanReset()
    {
        var viewModel = new MainWindowViewModel(new FakeModelApi());
        BallModelApi fakeBallModelApi = new FakeBallModelApi();
        viewModel.Balls.Add(fakeBallModelApi);
        Assert.IsTrue(viewModel.CanReset());
        viewModel.Balls.Clear();
        Assert.IsFalse(viewModel.CanReset());
    }
}