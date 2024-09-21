namespace ImageEditor.Models.Actions.Parameters;

public class ScaleParameter(string name, string description, Scale value, float minValue = 1, float maxValue = 200, float stepValue = 1)
    : ActionParameter<Scale>(name, description, value)
{
    public float MinValue { get; } = minValue;
    public float MaxValue { get; } = maxValue;
    public float StepValue { get; } = stepValue;
    
    public Dictionary<Scale.ScaleType, string> Options { get; } = new()
    {
        { Scale.ScaleType.Percent, "%" },
        { Scale.ScaleType.Pixel, "px" },
    };

    public override string ToString()
    {
        return Value.ToString();
    }
}