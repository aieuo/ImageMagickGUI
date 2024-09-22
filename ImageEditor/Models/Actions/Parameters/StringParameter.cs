namespace ImageEditor.Models.Actions.Parameters;

public class StringParameter(string name, string description, string value)
    : ActionParameter<string>(name, description, value)
{
    public override string ToString()
    {
        return Value;
    }
}