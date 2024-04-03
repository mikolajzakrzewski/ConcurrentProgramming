using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class Model : ModelAPI
    {
        private readonly Table table;
        public override int Width => table.Width;

        public override int Height => table.Height;

        public override void CreateBalls(int number, int radius)
        {
            table.CreateBalls(number, radius);
        }

        public override void Start()
        {
            table.Start();
        }

        public override void ResetTable()
        {
            table.ResetTable();
        }
    }
}
