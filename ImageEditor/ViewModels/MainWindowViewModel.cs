﻿using ImageEditor.Models.Actions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageEditor.Models;
using ImageEditor.Models.Deserializer;
using ImageEditor.Models.Serializer;
using ImageEditor.Utils;
using ImageEditor.ViewModels.DragDrop;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using Action = ImageEditor.Models.Actions.Action;

namespace ImageEditor.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isPopupOpen;

    [ObservableProperty]
    private Action? _selectedAction = null;

    [ObservableProperty]
    private Visibility _loadImageButtonVisibility = Visibility.Visible;

    [ObservableProperty]
    private string _sidePanelFooterMessage = "";
    
    [ObservableProperty]
    private string _imagePanelFooterRightMessage = "";
    
    public List<Action> Actions { get; }

    public ObservableCollection<Action> AddedActions { get; }

    public Image Image { get; } = new();

    public ImageDragDropHandler ImageDragDropHandler { get; private set; }
    public ActionDragDropHandler ActionDragDropHandler { get; private set; }

    private readonly IDialogService _dialogService;

    private readonly DebounceDispatcher _actionUpdateDebouncer = new();

    private string _saveImagePath = "";
    private string _saveActionPath = "";

    public MainWindowViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;

        Actions = ActionFactory.GetInstance().All();

        AddedActions = [];
        AddedActions.CollectionChanged += (t, a) => RequestProcessImage();

        Image.PropertyChanged += OnImageUpdated;

        ImageDragDropHandler = new ImageDragDropHandler(TryLoadImage);
        ActionDragDropHandler = new ActionDragDropHandler(TryLoadAction);
    }

    [RelayCommand]
    private void TogglePopup(bool open)
    {
        IsPopupOpen = open;
    }

    [RelayCommand]
    private void AddAction(string? name)
    {
        if (name == null) return;
        
        IsPopupOpen = false;

        var action = ActionFactory.GetInstance().Get(name);

        AddedActions.Add(action);
        action.PropertyChanged += (_, _) => OnUpdateAction();

        SidePanelFooterMessage = "追加しました";
    }

    [RelayCommand]
    private void DeleteAction()
    {
        if (SelectedAction == null)
        {
            SidePanelFooterMessage = "削除する対象が選択されていません";
            return;
        }

        var result = _dialogService.ShowMessageBox(
            this,
            $"本当に「{SelectedAction.FormatedString}」を削除しますか?",
            "削除",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question
        );
        if (result == MessageBoxResult.No)
        {
            SidePanelFooterMessage = "キャンセルしました";
            return;
        }

        AddedActions.Remove(SelectedAction);
        SidePanelFooterMessage = "削除しました";
    }

    [RelayCommand]
    private void LoadActions()
    {
        var settings = new OpenFileDialogSettings
        {
            Title = "ファイルを選択してください",
            Filter = "File|*.json",
        };

        var success = _dialogService.ShowOpenFileDialog(this, settings);
        if (success != true)
        {
            SidePanelFooterMessage = "キャンセルしました";
            return;
        }

        TryLoadAction(settings.FileName);
    }

    private void TryLoadAction(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            AddedActions.Clear();
            foreach (var action in ActionDeserializer.GetInstance().Deserialize(json))
            {
                AddedActions.Add(action);
                action.PropertyChanged += (_, _) => OnUpdateAction();
            }
        }
        catch (Exception e)
        {
            _dialogService.ShowMessageBox(
                this,
                $"ファイルの読み込みに失敗しました\n{e.Message}",
                icon: MessageBoxImage.Error
            );
            SidePanelFooterMessage = "読み込み失敗";
            return;
        }

        SidePanelFooterMessage = "読み込みました";
    }

    private void OnUpdateAction()
    {
        SidePanelFooterMessage = "";
        RequestProcessImage();
    }

    [RelayCommand]
    private void SaveActions(string? type)
    {
        if (type == null) return;
        
        if (type == "New" || _saveActionPath == "" || !Directory.Exists(Path.GetDirectoryName(_saveActionPath)))
        {
            var settings = new SaveFileDialogSettings
            {
                Title = "ファイルを選択してください",
                Filter = "File|*.json",
            };

            var success = _dialogService.ShowSaveFileDialog(this, settings);
            if (success != true)
            {
                SidePanelFooterMessage = "キャンセルしました";
                return;
            }

            _saveActionPath = settings.FileName;
        }

        try
        {
            var json = ActionSerializer.GetInstance().Serialize(AddedActions);
            File.WriteAllText(_saveActionPath, json);
        }
        catch (Exception e)
        {
            _dialogService.ShowMessageBox(
                this,
                $"ファイルの保存に失敗しました\n{e.Message}",
                icon: MessageBoxImage.Error
            );
            SidePanelFooterMessage = "保存失敗";
            return;
        }

        SidePanelFooterMessage = "保存しました";
    }

    private void OnImageUpdated(object? sender, PropertyChangedEventArgs e)
    {
        ImagePanelFooterRightMessage = e.PropertyName switch
        {
            nameof(Image.IsProcessingImage) => Image.IsProcessingImage ? "処理中..." : "処理完了",
            _ => ImagePanelFooterRightMessage
        };
    }

    [RelayCommand]
    private void LoadImage()
    {
        if (!ConfirmImageLoad())
        {
            return;
        }

        var settings = new OpenFileDialogSettings
        {
            Title = "画像を選択してください",
            Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif|All Files|*.*",
        };

        var success = _dialogService.ShowOpenFileDialog(this, settings);
        if (success != true)
        {
            ImagePanelFooterRightMessage = "キャンセルしました";
            return;
        }

        TryLoadImage(settings.FileName);
    }

    private void TryLoadImage(string path)
    {
        if (!ConfirmImageLoad())
        {
            return;
        }
        
        try
        {
            Image.Load(path);
        }
        catch (Exception e)
        {
            _dialogService.ShowMessageBox(
                this,
                $"ファイルの読み込みに失敗しました\n{e.Message}",
                icon: MessageBoxImage.Error
            );
            ImagePanelFooterRightMessage = "画像読み込み失敗";
            return;
        }

        ImagePanelFooterRightMessage = "読み込みました";

        RequestProcessImage();
        LoadImageButtonVisibility = Visibility.Hidden;
    }

    private bool ConfirmImageLoad()
    {
        if (!Image.IsChanged)
        {
            return true;
        }
        
        var result = _dialogService.ShowMessageBox(
            this,
            "保存されていない画像があります．\n読み込みますか?",
            icon: MessageBoxImage.Warning,
            button: MessageBoxButton.OKCancel
        );
        return result == MessageBoxResult.OK;
    }

    [RelayCommand]
    private void SaveImage(string? type)
    {
        if (type == null) return;
        
        if (Image.ProcessedImage == null)
        {
            ImagePanelFooterRightMessage = "保存する画像がありません";
            return;
        }

        if (type == "New" || _saveImagePath == "" || !Directory.Exists(Path.GetDirectoryName(_saveImagePath)))
        {
            var settings = new SaveFileDialogSettings
            {
                Title = "画像を選択してください",
                Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif|All Files|*.*",
            };

            var success = _dialogService.ShowSaveFileDialog(this, settings);
            if (success != true)
            {
                ImagePanelFooterRightMessage = "キャンセルしました";
                return;
            }

            _saveImagePath = settings.FileName;
        }

        try
        {
            Image.Save(_saveImagePath);
        }
        catch (Exception e)
        {
            _dialogService.ShowMessageBox(
                this,
                $"ファイルの保存に失敗しました\n{e.Message}",
                icon: MessageBoxImage.Error
            );
            ImagePanelFooterRightMessage = "画像保存失敗";
            return;
        }

        ImagePanelFooterRightMessage = "保存しました";
    }

    private void RequestProcessImage()
    {
        _actionUpdateDebouncer.Debounce(() =>
        {
            try
            {
                Image.Process(AddedActions).Wait();
            }
            catch (Exception)
            {
                ImagePanelFooterRightMessage = "画像処理中にエラーが発生しました";
            }
        });
    }

    [RelayCommand]
    private void SaveAll()
    {
        SaveImage("Override");
        SaveActions("Override");
    }
}