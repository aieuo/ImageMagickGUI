﻿using System.Windows.Controls;
using System.Windows;
using ImageEditor.Models.Actions.Parameters;

namespace ImageEditor.Utils;

internal class InputTemplateSelector : DataTemplateSelector
{
    public DataTemplate FloatParamTemplate { get; set; } = null!;
    public DataTemplate IntParamTemplate { get; set; } = null!;
    public DataTemplate StringParamTemplate { get; set; } = null!;
    public DataTemplate BooleanParamTemplate { get; set; } = null!;
    public DataTemplate EnumParamTemplate { get; set; } = null!;
    public DataTemplate ColorParamTemplate { get; set; } = null!;
    public DataTemplate ScaleParamTemplate { get; set; } = null!;
    public DataTemplate WidthAndHeightParamTemplate { get; set; } = null!;
    public DataTemplate PositionParamTemplate { get; set; } = null!;

    public override DataTemplate SelectTemplate(object? item, DependencyObject container)
    {
        return item switch
        {
            FloatParameter => FloatParamTemplate,
            IntParameter => IntParamTemplate,
            StringParameter => StringParamTemplate,
            BooleanParameter => BooleanParamTemplate,
            EnumParameter => EnumParamTemplate,
            ColorParameter => ColorParamTemplate,
            ScaleParameter => ScaleParamTemplate,
            WidthAndHeightParameter => WidthAndHeightParamTemplate,
            PositionParameter => PositionParamTemplate,
            _ => throw new NotImplementedException(),
        };
    }
}