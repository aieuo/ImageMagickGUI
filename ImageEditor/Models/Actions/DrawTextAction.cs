using System.Windows.Media;
using ImageEditor.Models.Actions.Parameters;
using ImageMagick;
using ImageMagick.Drawing;

namespace ImageEditor.Models.Actions;

internal class DrawTextAction : Action
{
    public override string Name => "Text";

    public override string Description => "文字を描画する";

    public override string FormatedString => "文字を描画する";

    public override string IconPath => "../../Resources/text-formatting.png";

    internal DrawTextAction() : this("", 16, Scale.Percent(0), Scale.Percent(0), Colors.Black)
    {
    }

    internal DrawTextAction(string text, float fontSize, Scale x, Scale y, Color color)
    {
        AddParameter(new StringParameter("text", "テキスト", text));
        AddParameter(new FloatParameter("fontSize", "フォントサイズ", fontSize, 1, 160));
        AddParameter(new PositionParameter("position", "位置", x, y));
        AddParameter(new ColorParameter("color", "塗りつぶし色", color));
    }

    public override void ProcessImage(MagickImage image)
    {
        var text = GetParameter<StringParameter>("text").Value;
        var position = GetParameter<PositionParameter>("position");
        var fontSize = GetParameter<FloatParameter>("fontSize").Value;
        var color = ParameterUtils.ColorParameterToMagickColor(GetParameter<ColorParameter>("color"), image.ColorSpace);

        if (text == "")
        {
            return;
        }

        var drawable = new Drawables();
        drawable.FillColor(color);
        drawable.FontPointSize(fontSize);
        drawable.Text(
            position.X.Value.ToPixel(image.Width),
            position.Y.Value.ToPixel(image.Width),
            text
        );
        
        image.Draw(drawable);
    }
}