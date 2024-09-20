using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageEditor.Models.Actions.Parameters;

public abstract partial class ActionParameter<T> : ActionParameter
{
    [ObservableProperty] private T _value;

    public ActionParameter(string name, string description, T value) : base(name, description)
    {
        Value = value;
    }

    partial void OnValueChanged(T value)
    {
        if (value is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged += (_, _) => OnPropertyChanged(nameof(Value));
        }
    }
}

public abstract class ActionParameter(string name, string description) : ObservableObject
{
    public string Name { get; } = name;
    public string Description { get; } = description;

    public abstract override string ToString();
}