using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Numerics;

namespace Model
{
    internal class BallModel : BallModelAPI, INotifyPropertyChanged
    {
        private Vector2 _position;
        private readonly int _radius;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BallModel(Vector2 position, int radius)
        {
            _position = position;
            _radius = radius;
        }

        public override float X
        {
            get => _position.X;
            set
            {
                _position.X = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public override float Y
        {
            get => _position.Y;
            set
            {
                _position.Y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public override int Radius
        {
            get => _radius;
        }
    }
}