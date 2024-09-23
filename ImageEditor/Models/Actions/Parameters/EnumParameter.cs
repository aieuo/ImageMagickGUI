using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageEditor.Models.Actions.Parameters;

public partial class EnumParameter<T>(string name, string description, T value, Dictionary<T, string> options)
    : EnumParameter(name, description, value, (options as Dictionary<Enum, string>)!) where T : Enum
{
    [ObservableProperty]
    private T _value = value;
    
    public Dictionary<T, string> Options => options;
    
    public override string ToString()
    {
        return Value.ToString();
    }
}

public abstract class EnumParameter(string name, string description, Enum value, Dictionary<Enum, string> options)
    : ActionParameter<Enum>(name, description, value)
{
}