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

    internal BorderAction(int width = 10, int height = 10) : this(width, height, Colors.Black)
    {
    }

    internal BorderAction(int width, int height, Color color)
    {
        AddParameter(new IntParameter("width", "枠線の幅", width, 0, 100));
        AddParameter(new IntParameter("height", "枠線の高さ", height, 0, 100));
        AddParameter(new ColorParameter("color", "余白の色", color));
    }

    public override void ProcessImage(MagickImage image)
    {
        var width = GetParameter<IntParameter>("width").Value;
        var height = GetParameter<IntParameter>("height").Value;
        var color = ParameterUtils.ColorParameterToMagickColor(GetParameter<ColorParameter>("color"));

        image.BorderColor = color;
        image.Border((uint)width, (uint)height);
    }
}