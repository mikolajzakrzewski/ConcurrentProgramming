using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Model
{

    internal class BallModel : BallModelAPI, INotifyPropertyChanged
    {
        private float _x;
        private float _y;
        private readonly int _radius;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BallModel(float x, float y, int radius)
        {
            _x = x;
            _y = y;
            _radius = radius;
        }

        public override float X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public override float Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public override int Radius
        {
            get => _radius;
        }
    }
}