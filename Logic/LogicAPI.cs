using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public abstract class LogicAPI : IObservable<LogicAPI>
    {
        public static LogicAPI Instance(int width, int height)
        {
            return new Table(width, height);
        }

        public static LogicAPI Instance(int width, int height, List<DataAPI> balls)
        {
            return new Table(width, height, balls);
        }

        public abstract int Width { get; }

        public abstract int Height { get; }

        public abstract List<List<float>> GetBallPositions();

        public abstract void CreateBalls(int number, int radius);

        public abstract void Start(float velocity);

        public abstract void ResetTable();

        public abstract IDisposable Subscribe(IObserver<LogicAPI> observer);
    }
}
