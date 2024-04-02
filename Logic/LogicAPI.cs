using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class LogicAPI
    {
        public abstract void CreateBalls(int number);

        public abstract void Start();

        public abstract List<List<int>> GetBallPositions();

        public abstract void ResetBoard();
    }
}
