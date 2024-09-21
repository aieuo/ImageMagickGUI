using System.Windows.Media;
using ImageEditor.Models.Actions.Parameters;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal class ExtentAction : Action
{
    private readonly Dictionary<Gravity, string> _allOptions = new()
    {
        { Gravity.Northwest, "左上" },
        { Gravity.North, "上" },
        { Gravity.Northeast, "右上" },
        { Gravity.West, "左" },
        { Gravity.Center, "中央" },
        { Gravity.East, "右" },
        { Gravity.Southwest, "左下" },
        { Gravity.South, "下" },
        { Gravity.Southeast, "右下" },
    };

    public override string Name => "Extent";

    public override string Description => "画像に余白を追加する";

    public override string FormatedString => "画像に余白を追加する";

    public override string IconPath => "../../Resources/boundary.png";

    internal ExtentAction() : this(new Scale(100), new Scale(100), Colors.White)
    {
    }

    internal ExtentAction(Scale width, Scale height, Color color, Gravity type = Gravity.Center)
    {
        AddParameter(new WidthAndHeightParameter("size", "サイズ", width, height));
        AddParameter(new ColorParameter("color", "余白の色", color));
        AddParameter(new EnumParameter<Gravity>("gravity", "基準点", type, _allOptions));
    }

    public override void ProcessImage(MagickImage image)
    {
        var size = ParameterUtils.WidthAndHeightParameterToGeometry(
            GetParameter<WidthAndHeightParameter>("size"),
            image.Width,
            image.Height
        );
        var color = ParameterUtils.ColorParameterToMagickColor(GetParameter<ColorParameter>("color"));
        var gravity = GetParameter<EnumParameter<Gravity>>("gravity").Value;

        image.Extent(size, gravity, color);
    }
}