using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
// TODO: replace IObserver<DataAPI> implementation
using Data;
//end

namespace Model
{
    // TODO: replace IObserver<DataAPI> implementation
    public class BallModel : INotifyPropertyChanged, IObserver<DataAPI>
    // end
    {
        private float _x;
        private float _y;
        private readonly int _radius;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // TODO: replace IObserver<DataAPI> implementation
        void IObserver<DataAPI>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        void IObserver<DataAPI>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DataAPI value)
        {
            _x = value.X;
            _y = value.Y;
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
        }

        // end

        public BallModel(float x, float y, int radius)
        {
            _x = x;
            _y = y;
            _radius = radius;
        }

        public float X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public float Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public int Radius
        {
            get => _radius;
        }
    }
}
