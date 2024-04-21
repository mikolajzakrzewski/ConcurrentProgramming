using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class BallModelAPI
    {
        public static BallModelAPI Instance(float x, float y, int radius)
        {
            return new BallModel(x, y, radius);
        }

        public abstract float X { get; set; }

        public abstract float Y { get; set; }

        public abstract int Radius { get; }
    }
}
