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
using System.Xml;
using System.Xml.Schema;
using ImageMagick;

namespace ImageEditor.Models.Actions;

internal abstract class Action : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public abstract string Name { get; }

    public abstract string Description { get; }
        
    public abstract string FormatedString { get; }

    /// <summary>
    /// /Views/MainWindow.xaml からの相対パス
    /// </summary>
    public abstract string IconPath { get; }

    public ObservableCollection<ActionParameter> Parameters { get; } = [];

    protected void AddParameter(ActionParameter parameter)
    {
        Parameters.Add(parameter);
        parameter.PropertyChanged += (_, _) =>
        {
            NotifyPropertyChanged(nameof(Parameters));
            NotifyPropertyChanged(nameof(FormatedString));
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

    public abstract Dictionary<string, string> GetCommandParameters();
    
    public abstract MagickImage ProcessImage(MagickImage image);
}