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
using System.Windows.Media.Imaging;
using ImageEditor.Models.Actions.Parameters;
using ImageMagick;
using Microsoft.Win32;
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
    

    private double _originalImageZoom = 1;

    public double OriginalImageZoom
    {
        get => _originalImageZoom;
        set
        {
            if (_originalImageZoom != value)
            {
                _originalImageZoom = value;
                NotifyPropertyChanged();
            }
        }
    }

    private double _processedImageZoom = 1;

    public double ProcessedImageZoom
    {
        get => _processedImageZoom;
        set
        {
            if (_processedImageZoom != value)
            {
                _processedImageZoom = value;
                NotifyPropertyChanged();
            }
        }
    }

    public MainWindowViewModel()
    {
        TogglePopupCommand = new DelegateCommand<bool>(TogglePopup);

        Actions = ActionFactory.GetInstance().All();
        AddedActions = [];
        AddActionCommand = new DelegateCommand<string>(AddAction);
        DeleteActionCommand = new DelegateCommand(DeleteAction);

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
        action.PropertyChanged += (_, _) => NotifyPropertyChanged(nameof(AddedActions));
    }

    private void DeleteAction()
    {
        ProcessImage();
    }

    private void ProcessImage()
    {
        if (OriginalImage == null) return;

        var tmp = (MagickImage) OriginalImage.Clone();
        foreach (var action in AddedActions)
        {
            action.ProcessImage(tmp);
        }

        ProcessedImage = tmp;
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
        LoadImageCommandVisibility = Visibility.Hidden;
    }
}