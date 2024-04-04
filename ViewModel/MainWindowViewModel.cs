using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Model;

namespace ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ModelAPI modelAPI;
        private readonly int _width;
        private readonly int _height;
        public int _ballsAmount;
        public double _velocity;
        private readonly ObservableCollection<BallModel> _balls;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel(BallModel ball)
        {
            modelAPI = ModelAPI.Instance();
            _width = modelAPI.Width;
            _height = modelAPI.Height;
            BallsAmount = 0;
            _balls = new ObservableCollection<BallModel>();
        }

        public ObservableCollection<BallModel> Balls
        {
            get { return _balls; }
        }

        public int BallsAmount
        {
            get { return _ballsAmount; }
            set
            {
                if (_ballsAmount != value)
                {
                    _ballsAmount = value;
                    OnPropertyChanged(nameof(BallsAmount));
                }
            }
        }

        public double Velocity
        {
            get { return _velocity; }
            set
            {
                if (value != _velocity)
                {
                    _velocity = value;
                    OnPropertyChanged(nameof(Velocity));
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
