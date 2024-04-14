using Model;
using ViewModel;

namespace ViewModelTest
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public void TestCanCreateBalls()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            viewModel.Balls.Add(new BallModel(0, 0, 5));
            Assert.IsFalse(viewModel.CanCreateBalls());

            viewModel.BallsAmount = 0;
            Assert.IsFalse(viewModel.CanCreateBalls());
        }

        [TestMethod]
        public void TestCanStart()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            viewModel.Balls.Add(new BallModel(0, 0, 5));
            viewModel.Velocity = 5;

            Assert.IsTrue(viewModel.CanStart());
        }

        [TestMethod]
        public void TestCanReset()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            viewModel.Balls.Add(new BallModel(0, 0, 5));
            Assert.IsTrue(viewModel.CanReset());

            viewModel.Balls.Clear();
            Assert.IsFalse(viewModel.CanReset());
        }
    }
}