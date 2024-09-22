using System.Windows.Media;
using ImageEditor.Models.Actions.Parameters;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class CropAction : Action
{
    public override string Name => "Crop";

    public override string Description => "画像を切り取る";

    public override string FormatedString => "画像を切り取る";

    public override string IconPath => "../../Resources/crop.png";

    internal CropAction() : this(Scale.Percent(100), Scale.Percent(100), Scale.Pixel(0), Scale.Pixel(0))
    {
    }

    internal CropAction(Scale width, Scale height, Scale x, Scale y, Gravity type = Gravity.Center)
    {
        AddParameter(new WidthAndHeightParameter("size", "サイズ", width, height));
        AddParameter(new PositionParameter("offset", "オフセット", x, y));
        AddParameter(new EnumParameter<Gravity>("gravity", "基準点", type, ParameterUtils.GravityOptions));
    }

    public override void ProcessImage(MagickImage image)
    {
        var size = ParameterUtils.WidthAndHeightParameterToGeometry(
            GetParameter<WidthAndHeightParameter>("size"),
            image.Width,
            image.Height
        );
        var offset = GetParameter<PositionParameter>("offset");
        var gravity = GetParameter<EnumParameter<Gravity>>("gravity").Value;

        size.X = (int)offset.X.Value.ToPixel(image.Width);
        size.Y = (int)offset.Y.Value.ToPixel(image.Height);

        image.Crop(size, gravity);
    }
}