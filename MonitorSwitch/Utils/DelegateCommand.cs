using System;
using System.Windows.Input;

namespace MonitorSwitch.Utils;

public class DelegateCommand : ICommand
{
    private readonly Action<object?> _onExecute;
    private readonly Predicate<object?>? _onCanExecute;

    public DelegateCommand(Action<object?> onExecute, Predicate<object?>? onCanExecute = null)
    {
        _onExecute = onExecute;
        _onCanExecute = onCanExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _onCanExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        _onExecute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}