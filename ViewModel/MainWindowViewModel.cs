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
        public int _ballsAmount;
        public double _velocity;
        public RelayCommand StartButtonClicked { get; set; }
        public RelayCommand ResetButtonClicked { get; set; }

        public ObservableCollection<BallModel> _balls;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanStart()
        {
            if (BallsAmount > 0 && Velocity > 0 && Balls.Count == 0)
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
            if (Balls.Count > 0)
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
            _balls = modelAPI.Balls;
            StartButtonClicked = new RelayCommand(o => { CreateBalls(BallsAmount, (int)Velocity); }, o => CanStart());
            ResetButtonClicked = new RelayCommand(o => { ResetTable(); }, o => CanReset());
        }

        public void CreateBalls(int number, int radius)
        {
            modelAPI.CreateBalls(number, radius);
            StartButtonClicked.RaiseCanExecuteChanged();
            ResetButtonClicked.RaiseCanExecuteChanged();
        }

        public void ResetTable()
        { 
            modelAPI.ResetTable();
            StartButtonClicked.RaiseCanExecuteChanged();
            ResetButtonClicked.RaiseCanExecuteChanged();
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
