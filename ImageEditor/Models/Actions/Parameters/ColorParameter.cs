using System.Windows.Media;

namespace ImageEditor.Models.Actions.Parameters;

public class ColorParameter(string name, string description, Color value)
    : ActionParameter<Color>(name, description, value)
{
    public override string ToString()
    {
        return Value.ToString();
    }
}