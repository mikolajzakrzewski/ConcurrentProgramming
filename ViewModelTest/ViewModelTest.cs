using System.Collections.ObjectModel;
using Model;
using ViewModel;

namespace ViewModelTest;

internal class FakeModelApi : ModelApi
{
    public override int Width => 1;

    public override int Height => 1;

    public override ObservableCollection<BallModelApi> Balls => throw new NotImplementedException();

    public override void Start(int number, int radius, float velocity)
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
    public void CanStartTest()
    {
        var viewModel = new MainWindowViewModel(new FakeModelApi())
        {
            BallsAmount = 1,
            Velocity = 1,
            Radius = 1
        };
        Assert.IsTrue(viewModel.CanStart());
    }

    [TestMethod]
    public void CanResetTest()
    {
        var viewModel = new MainWindowViewModel(new FakeModelApi());
        BallModelApi fakeBallModelApi = new FakeBallModelApi();
        viewModel.Balls.Add(fakeBallModelApi);
        viewModel.Start(1, 1, 1);
        Assert.IsTrue(viewModel.CanReset());
        viewModel.Balls.Clear();
        Assert.IsFalse(viewModel.CanReset());
    }
}