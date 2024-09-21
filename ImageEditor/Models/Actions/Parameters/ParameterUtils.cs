using ImageMagick;

namespace ImageEditor.Models.Actions.Parameters;

public static class ParameterUtils
{
    public static readonly Dictionary<Gravity, string> GravityOptions = new()
    {
        { Gravity.Northwest, "左上" },
        { Gravity.North, "上" },
        { Gravity.Northeast, "右上" },
        { Gravity.West, "左" },
        { Gravity.Center, "中央" },
        { Gravity.East, "右" },
        { Gravity.Southwest, "左下" },
        { Gravity.South, "下" },
        { Gravity.Southeast, "右下" },
    };
    
    public static MagickGeometry WidthAndHeightParameterToGeometry(WidthAndHeightParameter parameter, double imageWidth,
        double imageHeight)
    {
        var width = parameter.Width.Value;
        var height = parameter.Height.Value;

        return width.Type switch
        {
            Scale.ScaleType.Percent when height.Type == Scale.ScaleType.Percent =>
                new MagickGeometry(new Percentage(width.Value), new Percentage(height.Value)),
            Scale.ScaleType.Pixel when height.Type == Scale.ScaleType.Pixel =>
                new MagickGeometry(
                    new Percentage(width.Value / imageWidth * 100),
                    new Percentage(height.Value / imageHeight * 100)
                ),
            Scale.ScaleType.Percent when height.Type == Scale.ScaleType.Pixel =>
                new MagickGeometry(new Percentage(width.Value), new Percentage(height.Value / imageHeight * 100)),
            Scale.ScaleType.Pixel when height.Type == Scale.ScaleType.Percent =>
                new MagickGeometry(new Percentage(width.Value / imageWidth * 100), new Percentage(height.Value)),
            _ => throw new ArgumentException()
        };
    }

    public static MagickColor ColorParameterToMagickColor(ColorParameter parameter)
    {
        var color = parameter.Value;
        return new MagickColor(color.R, color.G, color.B, color.A);
    }
}