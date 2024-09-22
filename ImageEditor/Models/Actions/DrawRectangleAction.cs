using System.Windows.Media;
using ImageEditor.Models.Actions.Parameters;
using ImageMagick;
using ImageMagick.Drawing;

namespace ImageEditor.Models.Actions;

internal class DrawRectangleAction : Action
{
    public override string Name => "Rectangle";

    public override string Description => "長方形を描画する";

    public override string FormatedString => "長方形を描画する";

    public override string IconPath => "../../Resources/rectangular-shape-outline.png";

    internal DrawRectangleAction() : this(Scale.Percent(50), Scale.Percent(50), Scale.Pixel(0), Scale.Pixel(0), Colors.Black)
    {
    }

    internal DrawRectangleAction(Scale width, Scale height, Scale x, Scale y, Color color)
    {
        AddParameter(new WidthAndHeightParameter("size", "サイズ", width, height));
        AddParameter(new PositionParameter("offset", "オフセット", x, y));
        AddParameter(new ColorParameter("color", "塗りつぶし色", color));
    }

    public override void ProcessImage(MagickImage image)
    {
        var size = GetParameter<WidthAndHeightParameter>("size");
        var offset = GetParameter<PositionParameter>("offset");
        var color = ParameterUtils.ColorParameterToMagickColor(GetParameter<ColorParameter>("color"), image.ColorSpace);

        var drawable = new Drawables();
        drawable.FillColor(color);
        drawable.Rectangle(
            offset.X.Value.ToPixel(image.Width),
            offset.Y.Value.ToPixel(image.Width),
            offset.X.Value.ToPixel(image.Width) + size.Width.Value.ToPixel(image.Width),
            offset.Y.Value.ToPixel(image.Width) + size.Height.Value.ToPixel(image.Width)
        );
        
        image.Draw(drawable);
    }
}