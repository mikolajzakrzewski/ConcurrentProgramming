﻿using System;
using System.Windows.Input;

namespace ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            this._execute(parameter);
        }
    }
}
