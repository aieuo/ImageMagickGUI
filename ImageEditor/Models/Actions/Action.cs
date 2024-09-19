using ImageEditor.Models.Actions.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using System.Xml.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageMagick;

namespace ImageEditor.Models.Actions;

public abstract class Action : ObservableObject
{
    public abstract string Name { get; }

    public abstract string Description { get; }
    
    public abstract string FormatedString { get; }

    /// <summary>
    /// /Views/Components/SidePanel.xaml からの相対パス
    /// </summary>
    public abstract string IconPath { get; }

    public ObservableCollection<ActionParameter> Parameters { get; } = [];

    protected void AddParameter(ActionParameter parameter)
    {
        Parameters.Add(parameter);
        parameter.PropertyChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(FormatedString));
        };
    }

    protected T GetParameter<T>(string parameterName) where T : ActionParameter
    {
        if (Parameters.SingleOrDefault(v => v.Name == parameterName) is not T param)
        {
            throw new ArgumentException($"Parameter {parameterName} was not found");
        }

        return param;
    }
    
    public abstract void ProcessImage(MagickImage image);
}