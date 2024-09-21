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

    internal CropAction() : this(new Scale(100), new Scale(100))
    {
    }

    internal CropAction(Scale width, Scale height, Gravity type = Gravity.Center)
    {
        AddParameter(new WidthAndHeightParameter("size", "サイズ", width, height));
        AddParameter(new EnumParameter<Gravity>("gravity", "基準点", type, ParameterUtils.GravityOptions));
    }

    public override void ProcessImage(MagickImage image)
    {
        var size = ParameterUtils.WidthAndHeightParameterToGeometry(
            GetParameter<WidthAndHeightParameter>("size"),
            image.Width,
            image.Height
        );
        var gravity = GetParameter<EnumParameter<Gravity>>("gravity").Value;

        image.Crop(size, gravity);
    }
}