using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataAPI : IObservable<DataAPI>
    {
        public static DataAPI Instance(Vector2 position, int radius, int mass)
        {
            return new Ball(position, radius, mass);
        }

        public abstract Vector2 Position { get; set; }

        public abstract Vector2 Velocity { get; set; }

        public abstract int Radius { get; }

        public abstract int Mass { get; }

        public abstract Task Move(float velocity);

        public abstract IDisposable Subscribe(IObserver<DataAPI> observer);
    }
}