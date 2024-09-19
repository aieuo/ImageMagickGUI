using System.Windows.Controls;
using System.Windows;
using ImageEditor.Models.Actions.Parameters;

namespace ImageEditor.Utils;

internal class InputTemplateSelector : DataTemplateSelector
{
    public DataTemplate FloatParamTemplate { get; set; }
    public DataTemplate IntParamTemplate { get; set; }
    public DataTemplate EnumParamTemplate { get; set; }
    public DataTemplate ColorParamTemplate { get; set; }

    public override DataTemplate SelectTemplate(object? item, DependencyObject container)
    {
        return item switch
        {
            FloatParameter => FloatParamTemplate,
            IntParameter => IntParamTemplate,
            EnumParameter => EnumParamTemplate,
            ColorParameter => ColorParamTemplate,
            _ => base.SelectTemplate(item, container)
        };
    }
}