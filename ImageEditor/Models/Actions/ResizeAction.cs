using ImageEditor.Models.Actions.Parameters;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class ResizeAction : Action
{
    public override string Name => "Resize";

    public override string Description => "画像をリサイズする";

    public override string FormatedString => $"画像のサイズを{GetParameter<WidthAndHeightParameter>("size")}にする";

    public override string IconPath => "../../Resources/resolution.png";

    internal ResizeAction() : this(Scale.Percent(100), Scale.Percent(100), true)
    {
    }

    internal ResizeAction(Scale width, Scale height, bool keepAspectRadio)
    {
        AddParameter(new WidthAndHeightParameter("size", "サイズ", width, height));
        AddParameter(new BooleanParameter("keepAspectRadio", "縦横比を維持する", keepAspectRadio));
    }

    public override void ProcessImage(MagickImage image)
    {
        var size = GetParameter<WidthAndHeightParameter>("size");
        var keepAspectRadio = GetParameter<BooleanParameter>("keepAspectRadio").Value;

        image.Resize(ParameterUtils.WidthAndHeightParameterToGeometry(size, image.Width, image.Height, keepAspectRadio));
    }
}