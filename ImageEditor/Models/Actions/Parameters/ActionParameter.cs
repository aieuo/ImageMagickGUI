using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Models.Actions.Parameters;

public class ActionParameter<T>(string name, string description, T value)
    : ActionParameter(name, description)
{
    private T _value = value;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            NotifyPropertyChanged();
        }
    }
}

public class ActionParameter(string name, string description) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string Name { get; } = name;
    public string Description { get; } = description;
}