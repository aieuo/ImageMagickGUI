namespace ImageEditor.Models.Actions.Parameters;

public class BooleanParameter(string name, string description, bool value)
    : ActionParameter<bool>(name, description, value)
{
    public override string ToString()
    {
        return Value.ToString();
    }
}