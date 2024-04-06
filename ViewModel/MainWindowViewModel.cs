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
        public int _radius;
        public RelayCommand CreateBallsButtonClicked { get; set; }
        public RelayCommand StartButtonClicked { get; set; }
        public RelayCommand ResetButtonClicked { get; set; }

        public ObservableCollection<BallModel> _balls;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanCreateBalls()
        {
            if (BallsAmount > 0 && Radius > 0 && Balls.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanStart()
        {
            if (Balls.Count > 0 && Velocity > 0)
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
            CreateBallsButtonClicked = new RelayCommand(o => { CreateBalls(BallsAmount, Radius); }, o => CanCreateBalls()); 
            StartButtonClicked = new RelayCommand(o => { Start(Velocity); }, o => CanStart());
            ResetButtonClicked = new RelayCommand(o => { ResetTable(); }, o => CanReset());
        }

        public void CreateBalls(int number, int radius)
        {
            modelAPI.CreateBalls(number, radius);
            CreateBallsButtonClicked.RaiseCanExecuteChanged();
            StartButtonClicked.RaiseCanExecuteChanged();
            ResetButtonClicked.RaiseCanExecuteChanged();
        }

        public void Start(double Velocity)
        {
            modelAPI.Start(Velocity);
            CreateBallsButtonClicked.RaiseCanExecuteChanged();
            StartButtonClicked.RaiseCanExecuteChanged();
            ResetButtonClicked.RaiseCanExecuteChanged();
        }

        public void ResetTable()
        { 
            modelAPI.ResetTable();
            CreateBallsButtonClicked.RaiseCanExecuteChanged();
            StartButtonClicked.RaiseCanExecuteChanged();
            ResetButtonClicked.RaiseCanExecuteChanged();
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
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
                    CreateBallsButtonClicked.RaiseCanExecuteChanged();
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

        public int Radius
        { 
            get { return _radius; } 
            set
            {
                _radius = value;
                OnPropertyChanged(nameof(Radius));
                CreateBallsButtonClicked.RaiseCanExecuteChanged();
                StartButtonClicked.RaiseCanExecuteChanged();
            }
        }
    }
}
