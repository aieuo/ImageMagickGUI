using System.Numerics;

namespace ImageEditor.Models.Actions.Parameters;

using Position = KeyValuePair<ScaleParameter, ScaleParameter>;

public class PositionParameter : ActionParameter<Position>
{
    public ScaleParameter X => Value.Key;

    public ScaleParameter Y => Value.Value;

    public PositionParameter(string name, string description, Scale x, Scale y)
        : this(
            name,
            description,
            new ScaleParameter("x", "x", x, 0, 100),
            new ScaleParameter("y", "y", y, 0, 100)
        )
    {
    }

    public PositionParameter(string name, string description, ScaleParameter x, ScaleParameter y)
        : base(name, description, new Position(x, y))
    {
        x.PropertyChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(X));
        };
        y.PropertyChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(Y));
        };
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}