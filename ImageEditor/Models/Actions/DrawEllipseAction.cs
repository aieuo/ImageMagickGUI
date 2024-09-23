using System.Windows.Media;
using ImageEditor.Models.Actions.Parameters;
using ImageMagick;
using ImageMagick.Drawing;

namespace ImageEditor.Models.Actions;

internal class DrawEllipseAction : Action
{
    public override string Name => "Ellipse";

    public override string Description => "楕円を描画する";

    public override string FormatedString => "楕円を描画する";

    public override string IconPath => "../../Resources/ellipse-outline-shape-variant.png";

    internal DrawEllipseAction() : this(Scale.Percent(10), Scale.Percent(10), Scale.Percent(50), Scale.Percent(50), Colors.Black)
    {
    }

    internal DrawEllipseAction(Scale width, Scale height, Scale x, Scale y, Color color)
    {
        AddParameter(new WidthAndHeightParameter("size", "サイズ", width, height));
        AddParameter(new PositionParameter("center", "中心", x, y));
        AddParameter(new ColorParameter("color", "塗りつぶし色", color));
    }

    public override void ProcessImage(MagickImage image)
    {
        var size = GetParameter<WidthAndHeightParameter>("size");
        var center = GetParameter<PositionParameter>("center");
        var color = ParameterUtils.ColorParameterToMagickColor(GetParameter<ColorParameter>("color"), image.ColorSpace);

        var drawable = new Drawables();
        drawable.FillColor(color);
        drawable.Ellipse(
            center.X.Value.ToPixel(image.Width),
            center.Y.Value.ToPixel(image.Height),
            size.Width.Value.ToPixel(image.Width) / 2,
            size.Height.Value.ToPixel(image.Height) / 2,
            0,
            360
        );
        
        image.Draw(drawable);
    }
}