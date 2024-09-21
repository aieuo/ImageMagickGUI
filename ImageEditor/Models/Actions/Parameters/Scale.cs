using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageEditor.Models.Actions.Parameters;

public partial class Scale(float value, Scale.ScaleType type = Scale.ScaleType.Percent) : ObservableObject
{
    public enum ScaleType
    {
        Pixel,
        Percent,
    }

    [ObservableProperty]
    private float _value = value;

    [ObservableProperty]
    private ScaleType _type = type;

    public float ToPixel(uint max)
    {
        return Type == ScaleType.Pixel ? Value : max * Value / 100;
    }

    public override string ToString()
    {
        return $"{Value}{(Type == ScaleType.Percent ? "%" : "")}";
    }
}