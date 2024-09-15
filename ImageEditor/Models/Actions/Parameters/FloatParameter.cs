namespace ImageEditor.Models.Actions.Parameters;

public class FloatParameter(string name, string description, in float value, float? minValue = null, float? maxValue = null)
    : ActionParameter<float>(name, description, value)
{
    public float? MinValue { get; } = minValue;
    public float? MaxValue { get; } = maxValue;

    public override string ToString()
    {
        return Value.ToString();
    }
}