using System;
using System.Windows.Input;

namespace AvaloniaApplication1.VmTools;

public class CommandVM : ICommand
{
    Action _action;

    public CommandVM(Action action)
    {
        _action = action;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }
    public event EventHandler? CanExecuteChanged;
    public void Execute(object? parameter)
    {
        _action();
    }
    
}