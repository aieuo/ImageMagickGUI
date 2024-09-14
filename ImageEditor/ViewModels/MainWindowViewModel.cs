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

        public List<Models.Actions.Action> Actions { get; set; }

        public ICommand AddActionCommand { get; private set; }

        public ObservableCollection<Models.Actions.Action> AddedActions { get; set; }

        public MainWindowViewModel()
        {
            TogglePopupCommand = new DelegateCommand<bool>(TogglePopup);

            Actions = ActionFactory.GetInstance().All();
            AddActionCommand = new DelegateCommand<string>(AddAction);

            AddedActions = new ObservableCollection<Models.Actions.Action>();
        }

        private void TogglePopup(bool open)
        {
            IsPopupOpen = open;
        }

        private void AddAction(string name)
        {
            IsPopupOpen = false;

            var action = ActionFactory.GetInstance().Get(name);

            AddedActions.Add(action);
        }
    }
}