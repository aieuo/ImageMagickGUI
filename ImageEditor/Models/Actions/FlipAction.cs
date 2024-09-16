﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ImageEditor.Models.Actions.Parameters;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class FlipAction : Action
{
    internal enum FlipType
    {
        Flip,
        Flop,
    }

    private readonly Dictionary<FlipType, string> _allOptions = new()
    {
        { FlipType.Flip, "上下反転させる" },
        { FlipType.Flop, "左右反転させる" },
    };


    public override string Name => "Flip";

    public override string Description => "画像を反転させる";

    public override string FormatedString =>
        _allOptions.GetValueOrDefault(GetParameter<EnumParameter<FlipType>>("type").Value) ?? "?";

    public override string IconPath => "../Resources/flip.png";

    internal FlipAction(FlipType type = FlipType.Flip)
    {
        AddParameter(new EnumParameter<FlipType>("type", "方向", type, _allOptions));
    }

    public override void ProcessImage(MagickImage image)
    {
        if (GetParameter<EnumParameter<FlipType>>("type").Value == FlipType.Flip)
        {
            image.Flip();
        }
        else
        {
            image.Flop();
        }
    }

    public override Dictionary<string, string> GetCommandParameters()
    {
        return new Dictionary<string, string>
        {
            { $"-{GetParameter<EnumParameter<FlipType>>("type").Value.ToString().ToLower()}", "" }
        };
    }
}