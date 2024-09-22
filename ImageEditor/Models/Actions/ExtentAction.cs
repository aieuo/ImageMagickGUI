using System.Windows.Media;
using ImageEditor.Models.Actions.Parameters;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class ExtentAction : Action
{
    public override string Name => "Extent";

    public override string Description => "キャンバスを拡大する";

    public override string FormatedString => "キャンバスを拡大する";

    public override string IconPath => "../../Resources/boundary.png";

    internal ExtentAction() : this(Scale.Percent(100), Scale.Percent(100), Colors.White)
    {
    }

    internal ExtentAction(Scale width, Scale height, Color color, Gravity type = Gravity.Center)
    {
        AddParameter(new WidthAndHeightParameter("size", "画像サイズ", width, height));
        AddParameter(new ColorParameter("color", "余白の色", color));
        AddParameter(new EnumParameter<Gravity>("gravity", "基準点", type, ParameterUtils.GravityOptions));
    }

    public override void ProcessImage(MagickImage image)
    {
        var size = ParameterUtils.WidthAndHeightParameterToGeometry(
            GetParameter<WidthAndHeightParameter>("size"),
            image.Width,
            image.Height
        );
        var color = ParameterUtils.ColorParameterToMagickColor(GetParameter<ColorParameter>("color"), image.ColorSpace);
        var gravity = GetParameter<EnumParameter<Gravity>>("gravity").Value;

        image.Extent(size, gravity, color);
    }
}