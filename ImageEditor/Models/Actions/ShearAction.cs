using ImageEditor.Models.Actions.Parameters;
using System.Windows.Media;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class ShearAction : Action
{
    public override string Name => "Shear";

    public override string Description => "画像を傾ける";

    public override string FormatedString => "画像を傾ける";

    public override string IconPath => "../../Resources/shear.png";

    internal ShearAction(float angleX = 0, float angleY = 0) : this(angleX, angleY, Colors.Transparent)
    {
    }

    internal ShearAction(float angleX, float angleY, Color color)
    {
        AddParameter(new FloatParameter("angleX", "角度 (X軸)", angleX, -45, 45));
        AddParameter(new FloatParameter("angleY", "角度 (Y軸)", angleY, -45, 45));
        AddParameter(new ColorParameter("color", "余白の色", color));
    }

    public override void ProcessImage(MagickImage image)
    {
        var angleX = GetParameter<FloatParameter>("angleX").Value;
        var angleY = GetParameter<FloatParameter>("angleY").Value;
        var color = ParameterUtils.ColorParameterToMagickColor(GetParameter<ColorParameter>("color"), image.ColorSpace);
        
        image.BackgroundColor = color;
        image.Shear(angleX, angleY);
    }
}