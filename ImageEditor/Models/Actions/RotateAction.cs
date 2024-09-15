using ImageEditor.Models.Actions.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class RotateAction : Action
{
    public override string Name => "Rotate";

    public override string Description => "画像を回転させる";

    public override string FormatedString => $"{GetParameter<FloatParameter>("angle")}度回転させる";

    public override string IconPath => "../Resources/rotation.png";

    internal RotateAction(float angle = 0)
    {
        AddParameter(new FloatParameter("angle", "角度", angle, 0, 360));
    }

    public override MagickImage ProcessImage(MagickImage image)
    {
        image.Rotate(GetParameter<FloatParameter>("angle").Value);
        return image;
    }

    public override Dictionary<string, string> GetCommandParameters()
    {
        return new Dictionary<string, string>
        {
            { "-rotate", $"+{GetParameter<FloatParameter>("angle")}" }
        };
    }
}