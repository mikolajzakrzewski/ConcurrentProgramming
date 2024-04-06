using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class LogicAPI
    {
        public static Table Instance(int width, int height)
        {
            return new Table(width, height);
        }

        public abstract void CreateBalls(int number, int radius);

        public abstract Task Start(double velocity);

        public abstract List<List<float>> GetBallPositions();

        public abstract void ResetTable();
    }
}
