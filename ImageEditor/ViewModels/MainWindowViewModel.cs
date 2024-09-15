using ImageEditor.Commands;
using ImageEditor.Models.Actions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ImageEditor.Models.Actions.Parameters;
using Action = ImageEditor.Models.Actions.Action;

namespace ImageEditor.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ICommand TogglePopupCommand { get; private set; }

        private bool _isPopupOpen;

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                if (_isPopupOpen != value)
                {
                    _isPopupOpen = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<Action> Actions { get; }

        public ICommand AddActionCommand { get; private set; }

        public ObservableCollection<Action> AddedActions { get; }

        public MainWindowViewModel()
        {
            TogglePopupCommand = new DelegateCommand<bool>(TogglePopup);

            Actions = ActionFactory.GetInstance().All();
            AddActionCommand = new DelegateCommand<string>(AddAction);

            AddedActions = [];
        }

        private void TogglePopup(bool open)
        {
            IsPopupOpen = open;
            foreach (var item in AddedActions)
            {
                Console.WriteLine(item);
                foreach (var v in item.Parameters)
                {
                    Console.WriteLine(v);
                }
            }
        }

        private void AddAction(string name)
        {
            IsPopupOpen = false;

            var action = ActionFactory.GetInstance().Get(name);

            AddedActions.Add(action);
            action.PropertyChanged += (_, _) => NotifyPropertyChanged("AddedActions");
        }
    }
}