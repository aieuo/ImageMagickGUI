namespace ImageEditor.Models.Actions.Parameters;

using WidthAndHeight = KeyValuePair<ScaleParameter, ScaleParameter>;

public class WidthAndHeightParameter : ActionParameter<WidthAndHeight>
{
    public ScaleParameter Width => Value.Key;

    public ScaleParameter Height => Value.Value;

    public WidthAndHeightParameter(string name, string description, ScaleParameter width, ScaleParameter height)
        : base(name, description, new WidthAndHeight(width, height))
    {
        width.PropertyChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(Width));
        };
        height.PropertyChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(Height));
        };
    }

    public override string ToString()
    {
        return $"{Width}x{Height}";
    }
}