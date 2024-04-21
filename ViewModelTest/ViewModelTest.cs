using Model;
using ViewModel;

namespace ViewModelTest
{
    internal class FakeBallModelAPI : BallModelAPI
    {
        public override float X { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override float Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int Radius => throw new NotImplementedException();
    }

    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public void TestCanCreateBalls()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            BallModelAPI fakeBallModelAPI = new FakeBallModelAPI();
            viewModel.Balls.Add(fakeBallModelAPI);
            Assert.IsFalse(viewModel.CanCreateBalls());
            viewModel.BallsAmount = 0;
            Assert.IsFalse(viewModel.CanCreateBalls());
        }

        [TestMethod]
        public void TestCanStart()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            BallModelAPI fakeBallModelAPI = new FakeBallModelAPI();
            viewModel.Balls.Add(fakeBallModelAPI);
            viewModel.Velocity = 5;
            Assert.IsTrue(viewModel.CanStart());
        }

        [TestMethod]
        public void TestCanReset()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            BallModelAPI fakeBallModelAPI = new FakeBallModelAPI();
            viewModel.Balls.Add(fakeBallModelAPI);
            Assert.IsTrue(viewModel.CanReset());
            viewModel.Balls.Clear();
            Assert.IsFalse(viewModel.CanReset());
        }
    }
}