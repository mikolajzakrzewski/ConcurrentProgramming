using System.Collections.ObjectModel;
using System.ComponentModel;
using Model;

namespace ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly ModelApi _modelApi;

    public ObservableCollection<BallModelApi> _balls;
    public int _ballsAmount;
    public int _radius;
    private bool _startButtonEnabled = true;
    public float _velocity;

    public MainWindowViewModel()
    {
        _modelApi = ModelApi.Instance();
        Width = _modelApi.Width;
        Height = _modelApi.Height;
        _balls = _modelApi.Balls;
        CreateBallsButtonClicked = new RelayCommand(o => { CreateBalls(BallsAmount, Radius); }, o => CanCreateBalls());
        StartButtonClicked = new RelayCommand(o => { Start(Velocity); }, o => CanStart());
        ResetButtonClicked = new RelayCommand(o => { ResetTable(); }, o => CanReset());
    }

    public MainWindowViewModel(ModelApi modelAPI)
    {
        _modelApi = modelAPI;
        Width = modelAPI.Width;
        Height = modelAPI.Height;
        _balls = [];
        CreateBallsButtonClicked = new RelayCommand(o => { CreateBalls(BallsAmount, Radius); }, o => CanCreateBalls());
        StartButtonClicked = new RelayCommand(o => { Start(Velocity); }, o => CanStart());
        ResetButtonClicked = new RelayCommand(o => { ResetTable(); }, o => CanReset());
    }

    public RelayCommand CreateBallsButtonClicked { get; set; }
    public RelayCommand StartButtonClicked { get; set; }
    public RelayCommand ResetButtonClicked { get; set; }

    public int Width { get; }

    public int Height { get; }

    public ObservableCollection<BallModelApi> Balls => _balls;

    public int BallsAmount
    {
        get => _ballsAmount;
        set
        {
            if (_ballsAmount == value) return;
            _ballsAmount = value;
            OnPropertyChanged(nameof(BallsAmount));
            CreateBallsButtonClicked.RaiseCanExecuteChanged();
            StartButtonClicked.RaiseCanExecuteChanged();
            ResetButtonClicked.RaiseCanExecuteChanged();
        }
    }

    public float Velocity
    {
        get => _velocity;
        set
        {
            if (value == _velocity) return;
            _velocity = value;
            OnPropertyChanged(nameof(Velocity));
            StartButtonClicked.RaiseCanExecuteChanged();
        }
    }

    public int Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            OnPropertyChanged(nameof(Radius));
            CreateBallsButtonClicked.RaiseCanExecuteChanged();
            StartButtonClicked.RaiseCanExecuteChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool CanCreateBalls()
    {
        return BallsAmount > 0 && Radius > 0 && Balls.Count == 0;
    }

    public bool CanStart()
    {
        return _startButtonEnabled && Balls.Count > 0 && Velocity > 0;
    }

    public bool CanReset()
    {
        return Balls.Count > 0;
    }

    public void CreateBalls(int number, int radius)
    {
        _modelApi.CreateBalls(number, radius);
        CreateBallsButtonClicked.RaiseCanExecuteChanged();
        StartButtonClicked.RaiseCanExecuteChanged();
        ResetButtonClicked.RaiseCanExecuteChanged();
    }

    public void Start(float Velocity)
    {
        _modelApi.Start(Velocity);
        CreateBallsButtonClicked.RaiseCanExecuteChanged();
        _startButtonEnabled = false;
        StartButtonClicked.RaiseCanExecuteChanged();
        ResetButtonClicked.RaiseCanExecuteChanged();
    }

    public void ResetTable()
    {
        _modelApi.ResetTable();
        CreateBallsButtonClicked.RaiseCanExecuteChanged();
        _startButtonEnabled = true;
        StartButtonClicked.RaiseCanExecuteChanged();
        ResetButtonClicked.RaiseCanExecuteChanged();
    }
}