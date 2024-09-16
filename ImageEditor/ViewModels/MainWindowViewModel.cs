using ImageEditor.Commands;
using ImageEditor.Models.Actions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ImageEditor.Models.Actions.Parameters;
using ImageEditor.Utils;
using ImageMagick;
using Microsoft.Win32;
using ObservableCollections;
using Action = ImageEditor.Models.Actions.Action;

namespace ImageEditor.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    public ICommand TogglePopupCommand { get; private set; }

    private bool _isPopupOpen;

    public bool IsPopupOpen
    {
        get => _isPopupOpen;
        set => SetProperty(ref _isPopupOpen, value);
    }

    public List<Action> Actions { get; }

    public ICommand AddActionCommand { get; private set; }
    public ICommand DeleteActionCommand { get; private set; }

    public ObservableCollection<Action> AddedActions { get; }

    private Action? _selectedAction = null;

    public Action? SelectedAction
    {
        get => _selectedAction;
        set => SetProperty(ref _selectedAction, value);
    }

    public ICommand LoadImageCommand { get; private set; }

    private Visibility _loadImageCommandVisibility = Visibility.Visible;

    public Visibility LoadImageCommandVisibility
    {
        get => _loadImageCommandVisibility;
        set => SetProperty(ref _loadImageCommandVisibility, value);
    }

    private MagickImage? _originalImage = null;

    private MagickImage? OriginalImage
    {
        get => _originalImage;
        set
        {
            SetProperty(ref _originalImage, value);
            NotifyPropertyChanged(nameof(OriginalBitmapImage));
        }
    }

    public BitmapSource? OriginalBitmapImage => OriginalImage?.ToBitmapSource();

    private MagickImage? _processedImage = null;

    private MagickImage? ProcessedImage
    {
        get => _processedImage;
        set
        {
            SetProperty(ref _processedImage, value);
            NotifyPropertyChanged(nameof(ProcessedBitmapImage));
        }
    }

    public BitmapSource? ProcessedBitmapImage => ProcessedImage?.ToBitmapSource();

    private readonly DebounceDispatcher _actionUpdateDebouncer = new DebounceDispatcher();

    private bool _processingImage = false;
    private bool _shouldProcessImage = false;
    
    public event EventHandler<MvvmMessageBoxEventArgs>? DeleteActionDialogRequest;

    private string _sidePanelFooterMessage = "";

    public string SidePanelFooterMessage
    {
        get => _sidePanelFooterMessage;
        private set => SetProperty(ref _sidePanelFooterMessage, value);
    }

    public MainWindowViewModel()
    {
        TogglePopupCommand = new DelegateCommand<bool>(TogglePopup);

        Actions = ActionFactory.GetInstance().All();
        AddActionCommand = new DelegateCommand<string>(AddAction);
        DeleteActionCommand = new DelegateCommand(DeleteAction);

        AddedActions = [];
        AddedActions.CollectionChanged += (t, a) => ProcessImageDebounce();

        LoadImageCommand = new DelegateCommand(LoadImage);
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
        action.PropertyChanged += (_, _) => OnUpdateAction();
        
        SidePanelFooterMessage = "追加しました";
    }

    private void OnUpdateAction()
    {
        NotifyPropertyChanged(nameof(AddedActions));
        
        SidePanelFooterMessage = "";
        ProcessImageDebounce();
    }

    private void DeleteAction()
    {
        if (SelectedAction == null)
        {
            SidePanelFooterMessage = "削除する対象が選択されていません";
            return;
        }

        DeleteActionDialogRequest?.Invoke(this, new MvvmMessageBoxEventArgs(result =>
            {
                if (result == MessageBoxResult.No)
                {
                    SidePanelFooterMessage = "キャンセルしました";
                    return;
                }

                AddedActions.Remove(SelectedAction);
                SidePanelFooterMessage = "削除しました";
            }, $"本当に「{SelectedAction.FormatedString}」を削除しますか?", "削除", MessageBoxButton.YesNo, MessageBoxImage.Question));
    }

    private void LoadImage()
    {
        var op = new OpenFileDialog
        {
            Title = "Select a picture",
            Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                     "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                     "Portable Network Graphic (*.png)|*.png"
        };
        if (op.ShowDialog() != true) return;

        OriginalImage = new MagickImage(op.FileName);
        ProcessImageDebounce();

        LoadImageCommandVisibility = Visibility.Hidden;
    }

    private async void ProcessImage()
    {
        if (OriginalImage == null) return;

        if (_processingImage)
        {
            _shouldProcessImage = true;
            return;
        }

        _processingImage = true;

        await Task.Run(() =>
        {
            var tmp = (MagickImage)OriginalImage.Clone();
            foreach (var action in AddedActions.ToList())
            {
                action.ProcessImage(tmp);
            }

            ProcessedImage = tmp;
        });

        _processingImage = false;
        
        if (_shouldProcessImage)
        {
            _shouldProcessImage = false;
            ProcessImageDebounce();
        }
    }

    private void ProcessImageDebounce()
    {
        _actionUpdateDebouncer.Debounce(ProcessImage);
    }
}