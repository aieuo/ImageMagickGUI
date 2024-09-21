using ImageEditor.Models.Actions.Parameters;
using System.Windows.Media;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class BorderAction : Action
{
    public override string Name => "Border";

    public override string Description => "枠線を追加する";

    public override string FormatedString => "枠線を追加する";

    public override string IconPath => "../../Resources/dot-square.png";

    internal BorderAction(int width = 10, int height = 10) : this(new Scale(width, Scale.ScaleType.Pixel), new Scale(height, Scale.ScaleType.Pixel), Colors.Black)
    {
    }

    internal BorderAction(Scale width, Scale height, Color color)
    {
        AddParameter(new ScaleParameter("width", "枠線の幅", width, 0, 100));
        AddParameter(new ScaleParameter("height", "枠線の高さ", height, 0, 100));
        AddParameter(new ColorParameter("color", "余白の色", color));
    }

    public override void ProcessImage(MagickImage image)
    {
        var width = GetParameter<ScaleParameter>("width").Value.ToPixel(image.Width);
        var height = GetParameter<ScaleParameter>("height").Value.ToPixel(image.Height);
        var color = ParameterUtils.ColorParameterToMagickColor(GetParameter<ColorParameter>("color"));

        image.BorderColor = color;
        image.Border((uint)width, (uint)height);
    }
}