using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageEditor.Commands;

internal class RelayCommand<T>(Action<T> execute, Func<bool> canExecute) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action<T> execute) : this(execute, () => true)
    {
    }

    public void Execute(object? parameter)
    {
        execute((T)parameter!);
    }

    public bool CanExecute(object? parameter)
    {
        return canExecute();
    }
}


internal class RelayCommand(Action execute, Func<bool> canExecute) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action execute) : this(execute, () => true)
    {
    }

    public void Execute(object? parameter)
    {
        execute();
    }

    public bool CanExecute(object? parameter)
    {
        return canExecute();
    }
}
