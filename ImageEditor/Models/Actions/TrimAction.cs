using ImageEditor.Models.Actions.Parameters;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class TrimAction : Action
{
    public override string Name => "Trim";

    public override string Description => "画像の余白を削除する";

    public override string FormatedString => "画像の余白を削除する";

    public override string IconPath => "../../Resources/cut.png";

    internal TrimAction(float fuzz = 0)
    {
        AddParameter(new FloatParameter("fuzz", "感度 (%)", fuzz, 0, 100));
    }

    public override void ProcessImage(MagickImage image)
    {
        image.ColorFuzz = new Percentage(GetParameter<FloatParameter>("fuzz").Value);
        image.Trim();
    }
}