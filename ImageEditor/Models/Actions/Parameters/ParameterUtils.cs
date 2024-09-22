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
    
    public static MagickGeometry WidthAndHeightParameterToGeometry(WidthAndHeightParameter parameter, double imageWidth, double imageHeight)
    {
        var width = parameter.Width.Value;
        var height = parameter.Height.Value;

        var geometry = width.Type switch
        {
            Scale.ScaleType.Percent when height.Type == Scale.ScaleType.Percent =>
                new MagickGeometry(new Percentage(width.Value), new Percentage(height.Value)),
            Scale.ScaleType.Pixel when height.Type == Scale.ScaleType.Pixel =>
                new MagickGeometry((uint)width.Value, (uint)height.Value),
            Scale.ScaleType.Percent when height.Type == Scale.ScaleType.Pixel =>
                new MagickGeometry((uint)(imageWidth * width.Value / 100), (uint)height.Value),
            Scale.ScaleType.Pixel when height.Type == Scale.ScaleType.Percent =>
                new MagickGeometry((uint)width.Value, (uint)(imageHeight * height.Value / 100)),
            _ => throw new ArgumentException()
        };
        geometry.IgnoreAspectRatio = true;
        
        return geometry;
    }

    public static MagickColor ColorParameterToMagickColor(ColorParameter parameter, ColorSpace space)
    {
        var color = parameter.Value;
        var rgbMagickColor = new MagickColor(color.R, color.G, color.B, color.A);

        using var tmp = new MagickImage(rgbMagickColor, 1, 1);
        tmp.ColorSpace = space;

        return (MagickColor) tmp.GetPixels().GetPixel(0, 0).ToColor()!;
    }
}