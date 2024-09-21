﻿using ImageEditor.Models.Actions.Parameters;
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
        AddParameter(new WidthAndHeightParameter("size", "サイズ", width, height));
    }

    public override void ProcessImage(MagickImage image)
    {
        var size = GetParameter<WidthAndHeightParameter>("size");

        image.Resize(ParameterUtils.WidthAndHeightParameterToGeometry(size, image.Width, image.Height));
    }
}