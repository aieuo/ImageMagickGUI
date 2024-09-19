namespace ImageEditor.Models.Actions.Parameters;

public class FloatParameter(string name, string description, float value, float minValue, float maxValue, float stepValue = 1)
    : ActionParameter<float>(name, description, value)
{
    public float MinValue { get; } = minValue;
    public float MaxValue { get; } = maxValue;
    public float StepValue { get; } = stepValue;

    public override string ToString()
    {
        return Value.ToString();
    }
}