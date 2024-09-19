namespace ImageEditor.Models.Actions.Parameters;

public class IntParameter(string name, string description, int value, int minValue, int maxValue, int stepValue = 1)
    : ActionParameter<int>(name, description, value)
{
    public int MinValue { get; } = minValue;
    public int MaxValue { get; } = maxValue;
    public int StepValue { get; } = stepValue;

    public override string ToString()
    {
        return Value.ToString();
    }
}