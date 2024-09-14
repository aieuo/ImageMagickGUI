namespace ImageEditor.Models.Actions.Parameters;

public class FloatParameter(string type, string description, in float value, float? minValue = null, float? maxValue = null)
    : ActionParameter<float>(type, description, in value)
{
    public float? MinValue { get; } = minValue;
    public float? MaxValue { get; } = maxValue;
}