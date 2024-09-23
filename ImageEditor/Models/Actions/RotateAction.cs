using ImageEditor.Models.Actions.Parameters;
using System.Windows.Media;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class RotateAction : Action
{
    public override string Name => "Rotate";

    public override string Description => "画像を回転させる";

    public override string FormatedString => $"{GetParameter<FloatParameter>("angle")}度回転させる";

    public override string IconPath => "../../Resources/rotation.png";

    internal RotateAction(float angle = 0) : this(angle, Colors.Transparent)
    {
    }

    internal RotateAction(float angle, Color color)
    {
        AddParameter(new FloatParameter("angle", "角度 (度)", angle, 0, 360));
        AddParameter(new ColorParameter("color", "余白の色", color));
    }

    public override void ProcessImage(MagickImage image)
    {
        var color = ParameterUtils.ColorParameterToMagickColor(GetParameter<ColorParameter>("color"), image.ColorSpace);
        var angle = GetParameter<FloatParameter>("angle").Value;
        
        image.BackgroundColor = color;
        image.Rotate(angle);
    }
}