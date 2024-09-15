using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows;
using ImageEditor.Models.Actions.Parameters;

namespace ImageEditor.Utils
{
    internal class InputTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FloatParamTemplate { get; set; }
        public DataTemplate EnumParamTemplate { get; set; }

        public override DataTemplate SelectTemplate(object? item, DependencyObject container)
        {
            return item switch
            {
                FloatParameter => FloatParamTemplate,
                EnumParameter => EnumParamTemplate,
                _ => base.SelectTemplate(item, container)
            };
        }
    }
}
