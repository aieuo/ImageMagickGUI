using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageEditor.Models.Actions.Parameters;

public class ScaleParameter(string name, string description, Scale value)
    : ActionParameter<Scale>(name, description, value)
{
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