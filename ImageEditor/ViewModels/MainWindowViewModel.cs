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
    public ICommand DeleteActionCommand { get; private set; }

    public ObservableCollection<Action> AddedActions { get; }

    public ICommand LoadImageCommand { get; private set; }

    private Visibility _loadImageCommandVisibility = Visibility.Visible;

    public Visibility LoadImageCommandVisibility
    {
        get => _loadImageCommandVisibility;
        set
        {
            if (_loadImageCommandVisibility != value)
            {
                _loadImageCommandVisibility = value;
                NotifyPropertyChanged();
            }
        }
    }

    private MagickImage? _originalImage = null;

    private MagickImage? OriginalImage
    {
        get => _originalImage;
        set
        {
            if (_originalImage != value)
            {
                _originalImage = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(OriginalBitmapImage));
            }
        }
    }

    public BitmapSource? OriginalBitmapImage => OriginalImage?.ToBitmapSource();

    private MagickImage? _processedImage = null;

    private MagickImage? ProcessedImage
    {
        get => _processedImage;
        set
        {
            if (_processedImage != value)
            {
                _processedImage = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ProcessedBitmapImage));
            }
        }
    }

    public BitmapSource? ProcessedBitmapImage => ProcessedImage?.ToBitmapSource();

    private readonly DebounceDispatcher _actionUpdateDebouncer = new DebounceDispatcher();

    private bool _processingImage = false;
    private bool _shouldProcessImage = false;

    public MainWindowViewModel()
    {
        TogglePopupCommand = new DelegateCommand<bool>(TogglePopup);

        Actions = ActionFactory.GetInstance().All();
        AddActionCommand = new DelegateCommand<string>(AddAction);
        DeleteActionCommand = new DelegateCommand(DeleteAction);
        
        AddedActions = [];
        AddedActions.CollectionChanged += OnUpdateAction;

        LoadImageCommand = new DelegateCommand(LoadImage);
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
        action.PropertyChanged += (_, _) =>
        {
            OnUpdateAction();
            NotifyPropertyChanged(nameof(AddedActions));
        };
    }

    private void DeleteAction()
    {
        ProcessImage();
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
        OnUpdateAction();
        
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
        Console.WriteLine("ProcessImage");
        
        await Task.Run(() =>
        {
            var tmp = (MagickImage)OriginalImage.Clone();
            foreach (var action in AddedActions.ToList())
            {
                var start = DateTime.Now;
                action.ProcessImage(tmp);
                Console.Write($"{action}: ");
                Console.WriteLine((DateTime.Now - start).TotalMilliseconds);
            }

            ProcessedImage = tmp;
            Console.WriteLine("FinishProcessImage");
        });
        
        _processingImage = false;
        if (_shouldProcessImage)
        {
            _shouldProcessImage = false;
            OnUpdateAction();
        }
    }

    private void OnUpdateAction(object? sender = null, NotifyCollectionChangedEventArgs? args = null)
    {
        Console.WriteLine("OnUpdateAction");
        _actionUpdateDebouncer.Debounce(ProcessImage);
    }
}