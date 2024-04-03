using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class LogicAPI
    {
        public abstract void CreateBalls(int number, int radius);

        public abstract void Start();

        public abstract List<List<float>> GetBallPositions();

        public abstract void ResetTable();
    }
}
