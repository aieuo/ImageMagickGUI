using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageEditor.Models.Actions.Parameters;

public abstract partial class ActionParameter<T>(string name, string description, T value)
    : ActionParameter(name, description)
{
    [ObservableProperty]
    private T _value = value;
}

public abstract class ActionParameter(string name, string description) : ObservableObject
{
    public string Name { get; } = name;
    public string Description { get; } = description;

    public abstract override string ToString();
}