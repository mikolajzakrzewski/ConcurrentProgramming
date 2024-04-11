using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataAPI
    {
        public static Ball Instance(float x, float y, int radius)
        {
            return new Ball(x, y, radius);
        }

        public abstract float X { get; set; }

        public abstract float Y { get; set; }

        public abstract int Radius { get; }

        public abstract Task Move(float x, float y, double velocity);
    }
}
