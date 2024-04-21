using System.Collections.ObjectModel;
using Model;
using ViewModel;

namespace ViewModelTest
{
    internal class FakeModelAPI : ModelAPI
    {
        public override int Width => 2;

        public override int Height => 2;

        public override void CreateBalls(int number, int radius)
        {
            
        }

        public override void Start(float velocity)
        {
            
        }

        public override void ResetTable()
        {
            
        }

        public override ObservableCollection<BallModelAPI> Balls => throw new NotImplementedException();
    }

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
            MainWindowViewModel viewModel = new MainWindowViewModel(new FakeModelAPI());
            Assert.IsFalse(viewModel.CanCreateBalls());
            viewModel.BallsAmount = 2;
            viewModel.Radius = 2;
            Assert.IsTrue(viewModel.CanCreateBalls());
            BallModelAPI fakeBallModelAPI = new FakeBallModelAPI();
            viewModel.Balls.Add(fakeBallModelAPI);
            Assert.IsFalse(viewModel.CanCreateBalls());
        }

        [TestMethod]
        public void TestCanStart()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel(new FakeModelAPI());
            BallModelAPI fakeBallModelAPI = new FakeBallModelAPI();
            viewModel.Balls.Add(fakeBallModelAPI);
            viewModel.Velocity = 5;
            Assert.IsTrue(viewModel.CanStart());
        }

        [TestMethod]
        public void TestCanReset()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel(new FakeModelAPI());
            BallModelAPI fakeBallModelAPI = new FakeBallModelAPI();
            viewModel.Balls.Add(fakeBallModelAPI);
            Assert.IsTrue(viewModel.CanReset());
            viewModel.Balls.Clear();
            Assert.IsFalse(viewModel.CanReset());
        }
    }
}