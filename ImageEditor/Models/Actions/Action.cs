using ImageEditor.Models.Actions.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace ImageEditor.Models.Actions
{
    internal abstract class Action : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public abstract string Name { get; }

        public abstract string Description { get; }

        /// <summary>
        /// /Views/MainWindow.xaml からの相対パス
        /// </summary>
        public abstract string IconPath { get; }

        public List<ActionParameter> Parameters { get; } = [];

        public abstract Dictionary<string, string> GetCommandParameters();

        public abstract override string ToString();
    }
}