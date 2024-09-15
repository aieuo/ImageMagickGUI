namespace ImageEditor.Models.Actions.Parameters;

public class EnumParameter<T>(string name, string description, T value, Dictionary<T, string> options)
    : EnumParameter(name, description, value, (options as Dictionary<Enum, string>)!) where T : Enum
{
    private T _value = value;
    public new T Value
    {
        get => _value;
        set
        {
            _value = value;
            NotifyPropertyChanged();
        }
    }
    
    public new Dictionary<T, string> Options => options;
}

public class EnumParameter(string name, string description, Enum value, Dictionary<Enum, string> options)
    : ActionParameter<Enum>(name, description, value)
{
    public Dictionary<Enum, string> Options => options;
    
    public override string ToString()
    {
        return Value.ToString();
    }
}