using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataAPI : IObservable<DataAPI>
    {
        public static DataAPI Instance(float x, float y, int radius)
        {
            return new Ball(x, y, radius);
        }

        public abstract float X { get; set; }

        public abstract float Y { get; set; }

        public abstract float VelocityX { get; set; }

        public abstract float VelocityY { get; set; }

        public abstract int Radius { get; }

        public abstract void Move(float velocity);

        public abstract IDisposable Subscribe(IObserver<DataAPI> observer);
    }
}
