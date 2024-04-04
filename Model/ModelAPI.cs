using System.Collections.ObjectModel;

namespace Model
{
    public abstract class ModelAPI
    {
        public static ModelAPI Instance()
        {
            return new Model();
        }
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract ObservableCollection<BallModel> Balls { get; }
        public abstract void CreateBalls(int number, int radius);
        public abstract void Start();
        public abstract void ResetTable();
    }
}
