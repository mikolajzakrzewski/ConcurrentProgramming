using System.Windows.Input;

namespace ViewModel;

public class RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
    : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return canExecute == null || canExecute(parameter!);
    }

    public void Execute(object? parameter)
    {
        execute(parameter!);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}