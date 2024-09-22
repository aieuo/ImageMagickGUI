using ImageEditor.Models.Actions.Parameters;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class SetColorSpaceAction : Action
{
    public override string Name => "SetColorSpace";

    public override string Description => "画像の色空間を設定する";

    public override string FormatedString =>
        $"色空間を{GetParameter<EnumParameter<ColorSpace>>("colorspace").Value}に設定する";

    public override string IconPath => "../../Resources/color.png";

    internal SetColorSpaceAction(ColorSpace space = ColorSpace.sRGB)
    {
        AddParameter(new EnumParameter<ColorSpace>("colorspace", "色空間", space, Enum.GetValues(typeof(ColorSpace))
            .Cast<ColorSpace>()
            .Where(c => c != ColorSpace.Undefined)
            .ToDictionary(e => e, e => e.ToString()))
        );
    }

    public override void ProcessImage(MagickImage image)
    {
        var colorspace = GetParameter<EnumParameter<ColorSpace>>("colorspace").Value;

        image.ColorSpace = colorspace;
        Console.WriteLine(colorspace);
        Console.WriteLine(image.ColorSpace);
    }
}