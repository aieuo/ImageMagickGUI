using ImageEditor.Models.Actions.Parameters;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class ResizeAction : Action
{
    public override string Name => "Resize";

    public override string Description => "画像をリサイズする";

    public override string FormatedString => $"画像のサイズを{GetParameter<WidthAndHeightParameter>("size")}にする";

    public override string IconPath => "../../Resources/resolution.png";

    internal ResizeAction() : this(new Scale(100), new Scale(100))
    {
    }

    internal ResizeAction(Scale width, Scale height)
    {
        AddParameter(new WidthAndHeightParameter(
            "size",
            "サイズ",
            new ScaleParameter("width", "横幅", width),
            new ScaleParameter("height", "高さ", height)
        ));
    }

    public override void ProcessImage(MagickImage image)
    {
        var size = GetParameter<WidthAndHeightParameter>("size");
        var width = size.Width.Value;
        var height = size.Height.Value;

        switch (width.Type)
        {
            case Scale.ScaleType.Percent when height.Type == Scale.ScaleType.Percent:
                image.Resize(new Percentage(width.Value), new Percentage(height.Value));
                break;
            case Scale.ScaleType.Pixel when height.Type == Scale.ScaleType.Pixel:
                image.Resize(new Percentage(width.Value / image.Width * 100), new Percentage(height.Value / image.Height * 100));
                break;
            case Scale.ScaleType.Percent when height.Type == Scale.ScaleType.Pixel:
                image.Resize(new Percentage(width.Value), new Percentage(height.Value / image.Height * 100));
                break;
            case Scale.ScaleType.Pixel when height.Type == Scale.ScaleType.Percent:
                image.Resize(new Percentage(width.Value / image.Width * 100), new Percentage(height.Value));
                break;
        }
    }
}