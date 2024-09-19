using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ImageEditor.Models.Actions.Parameters;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class FilterAction : Action
{
    internal enum FilterType
    {
        Blur,
        GaussianBlur,
        Charcoal,
        Emboss,
        OilPaint,
        Kuwahara,
        Sketch,
        Sharpen,
    }

    private readonly Dictionary<FilterType, string> _allOptions = new()
    {
        { FilterType.Blur, "ぼかしフィルタ" },
        { FilterType.GaussianBlur, "ガウスぼかしフィルタ" },
        { FilterType.Emboss, "エンボスフィルタ" },
        { FilterType.Charcoal, "木炭画風フィルタ" },
        { FilterType.OilPaint, "油絵風フィルタ" },
        { FilterType.Kuwahara, "桑原フィルタ" },
        { FilterType.Sketch, "鉛筆画風フィルタ" },
        { FilterType.Sharpen, "シャープ化" },
    };

    public override string Name => "Filter";

    public override string Description => "画像にフィルタをかける";

    public override string FormatedString =>
        _allOptions.GetValueOrDefault(GetParameter<EnumParameter<FilterType>>("type").Value) ?? "?";

    public override string IconPath => "../../Resources/photo-filter.png";

    internal FilterAction(FilterType type = FilterType.Blur, float radius = 0, float sigma = 1)
    {
        AddParameter(new EnumParameter<FilterType>("type", "フィルタ", type, _allOptions));
        AddParameter(new FloatParameter("radius", "半径", radius, 0, 10, 0.1f));
        AddParameter(new FloatParameter("sigma", "強さ", sigma, 0, 10, 0.1f));
    }

    public override void ProcessImage(MagickImage image)
    {
        var type = GetParameter<EnumParameter<FilterType>>("type").Value;
        var radius = GetParameter<FloatParameter>("radius").Value;
        var sigma = GetParameter<FloatParameter>("sigma").Value;

        switch (type)
        {
            case FilterType.Blur:
                image.Blur(radius, sigma);
                break;
            case FilterType.GaussianBlur:
                image.GaussianBlur(radius, sigma);
                break;
            case FilterType.Charcoal:
                image.Charcoal(radius, sigma);
                break;
            case FilterType.Emboss:
                image.Emboss(radius, sigma);
                break;
            case FilterType.OilPaint:
                image.OilPaint(radius, sigma);
                break;
            case FilterType.Kuwahara:
                image.Kuwahara(radius, sigma);
                break;
            case FilterType.Sketch:
                image.Sketch(radius, sigma, 0.0f);
                break;
            case FilterType.Sharpen:
                image.Sharpen(radius, sigma);
                break;
        }
    }
}