using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ModelAPI modelAPI;
        private readonly int _width;
        private readonly int _height;
        public int _ballsAmount = 0;
        public double _velocity = 0;
        public RelayCommand StartButtonClicked { get; set; }
        public RelayCommand ResetButtonClicked { get; set; }

        private readonly ObservableCollection<BallModel> _balls;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanStart()
        {
            if (BallsAmount > 0 && Velocity > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanReset()
        {
            if (BallsAmount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public MainWindowViewModel()
        {
            modelAPI = ModelAPI.Instance();
            _width = modelAPI.Width;
            _height = modelAPI.Height;
            _balls = new ObservableCollection<BallModel>();
            StartButtonClicked = new RelayCommand(o => { modelAPI.Start(); }, o => CanStart());
            ResetButtonClicked = new RelayCommand(o => { modelAPI.ResetTable(); }, o => CanReset());
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
                    StartButtonClicked.RaiseCanExecuteChanged();
                    ResetButtonClicked.RaiseCanExecuteChanged();
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
                    StartButtonClicked.RaiseCanExecuteChanged();
                }
            }
        }
    }
}
