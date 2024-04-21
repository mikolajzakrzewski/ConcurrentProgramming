using Logic;
using System.Collections.ObjectModel;

namespace Model
{
    public abstract class ModelAPI
    {
        public static ModelAPI Instance()
        {
            return new Model();
        }

        public static ModelAPI Instance(LogicAPI table)
        {
            return new Model(table);
        }

        public abstract int Width { get; }

        public abstract int Height { get; }

        public abstract ObservableCollection<BallModelAPI> Balls { get; }

        public abstract void CreateBalls(int number, int radius);

        public abstract void Start(float velocity);

        public abstract void ResetTable();
    }
}
