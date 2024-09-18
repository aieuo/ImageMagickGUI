﻿using ImageEditor.Commands;
using ImageEditor.Models.Actions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ImageEditor.Models;
using ImageEditor.Models.Deserializer;
using ImageEditor.Models.Serializer;
using ImageEditor.Utils;
using ImageEditor.ViewModels.DragDrop;
using ImageMagick;
using Microsoft.Win32;
using Action = ImageEditor.Models.Actions.Action;

namespace ImageEditor.ViewModels;

internal class MainWindowViewModel : NotifyPropertyChangedObject
{
    #region Commands

    public ICommand TogglePopupCommand { get; private set; }

    public ICommand AddActionCommand { get; private set; }
    public ICommand DeleteActionCommand { get; private set; }

    public ICommand LoadImageCommand { get; private set; }
    public ICommand SaveImageCommand { get; private set; }

    public ICommand LoadActionsCommand { get; private set; }
    public ICommand SaveActionsCommand { get; private set; }

    public ICommand SaveAllCommand { get; private set; }

    #endregion


    #region Properties

    private bool _isPopupOpen;

    public bool IsPopupOpen
    {
        get => _isPopupOpen;
        set => SetProperty(ref _isPopupOpen, value);
    }

    public List<Action> Actions { get; }

    public ObservableCollection<Action> AddedActions { get; }

    private Action? _selectedAction = null;

    public Action? SelectedAction
    {
        get => _selectedAction;
        set => SetProperty(ref _selectedAction, value);
    }

    private Visibility _loadImageCommandVisibility = Visibility.Visible;

    public Visibility LoadImageCommandVisibility
    {
        get => _loadImageCommandVisibility;
        set => SetProperty(ref _loadImageCommandVisibility, value);
    }

    public Image Image { get; } = new();

    private string _sidePanelFooterMessage = "";

    public string SidePanelFooterMessage
    {
        get => _sidePanelFooterMessage;
        private set => SetProperty(ref _sidePanelFooterMessage, value);
    }

    private string _imagePanelFooterRightMessage = "";

    public string ImagePanelFooterRightMessage
    {
        get => _imagePanelFooterRightMessage;
        private set => SetProperty(ref _imagePanelFooterRightMessage, value);
    }

    public ImageDragDropHandler ImageDragDropHandler { get; private set; }
    public ActionDragDropHandler ActionDragDropHandler { get; private set; }

    #endregion


    private readonly DebounceDispatcher _actionUpdateDebouncer = new();

    private bool _processingImage = false;
    private bool _shouldProcessImage = false;
    public event EventHandler<MvvmMessageBoxEventArgs>? MessageBoxRequest;

    private string _saveImagePath = "";
    private string _saveActionPath = "";

    public MainWindowViewModel()
    {
        TogglePopupCommand = new RelayCommand<bool>(TogglePopup);

        Actions = ActionFactory.GetInstance().All();
        AddActionCommand = new RelayCommand<string>(AddAction);
        DeleteActionCommand = new RelayCommand(DeleteAction);

        AddedActions = [];
        AddedActions.CollectionChanged += (t, a) => RequestProcessImage();

        Image.PropertyChanged += OnImageUpdated;

        LoadImageCommand = new RelayCommand(LoadImage);
        SaveImageCommand = new RelayCommand<string>(SaveImage);

        LoadActionsCommand = new RelayCommand(LoadActions);
        SaveActionsCommand = new RelayCommand<string>(SaveActions);

        SaveAllCommand = new RelayCommand(SaveAll);

        ImageDragDropHandler = new ImageDragDropHandler(TryLoadImage);
        ActionDragDropHandler = new ActionDragDropHandler(TryLoadAction);
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
        RequestProcessImage();
    }

    private void DeleteAction()
    {
        if (SelectedAction == null)
        {
            SidePanelFooterMessage = "削除する対象が選択されていません";
            return;
        }

        MessageBoxRequest?.Invoke(this, new MvvmMessageBoxEventArgs(result =>
            {
                if (result == MessageBoxResult.No)
                {
                    SidePanelFooterMessage = "キャンセルしました";
                    return;
                }

                AddedActions.Remove(SelectedAction);
                SidePanelFooterMessage = "削除しました";
            }, $"本当に「{SelectedAction.FormatedString}」を削除しますか?", "削除", MessageBoxButton.YesNo,
            MessageBoxImage.Question));
    }

    private void LoadActions()
    {
        var dialog = new OpenFileDialog
        {
            Title = "ファイルを選択してください",
            Filter = "File|*.json"
        };
        if (dialog.ShowDialog() != true)
        {
            SidePanelFooterMessage = "キャンセルしました";
            return;
        }

        TryLoadAction(dialog.FileName);
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
            MessageBoxRequest?.Invoke(this, new MvvmMessageBoxEventArgs(
                null,
                $"ファイルの読み込みに失敗しました\n{e.Message}",
                icon: MessageBoxImage.Error
            ));
            SidePanelFooterMessage = "読み込み失敗";
            return;
        }

        SidePanelFooterMessage = "読み込みました";
    }

    private void SaveActions(string type)
    {
        if (type == "New" || _saveActionPath == "" || !Directory.Exists(Path.GetDirectoryName(_saveActionPath)))
        {
            var dialog = new SaveFileDialog
            {
                Title = "ファイルを選択してください",
                Filter = "File|*.json"
            };
            if (dialog.ShowDialog() != true)
            {
                SidePanelFooterMessage = "キャンセルしました";
                return;
            }

            _saveActionPath = dialog.FileName;
        }

        try
        {
            var json = ActionSerializer.GetInstance().Serialize(AddedActions);
            File.WriteAllText(_saveActionPath, json);
        }
        catch (Exception e)
        {
            MessageBoxRequest?.Invoke(this, new MvvmMessageBoxEventArgs(
                null,
                $"ファイルの保存に失敗しました\n{e.Message}",
                icon: MessageBoxImage.Error
            ));
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

    private void LoadImage()
    {
        var dialog = new OpenFileDialog
        {
            Title = "画像を選択してください",
            Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif|All Files|*.*"
        };
        if (dialog.ShowDialog() != true)
        {
            ImagePanelFooterRightMessage = "キャンセルしました";
            return;
        }

        TryLoadImage(dialog.FileName);
    }

    private void TryLoadImage(string path)
    {
        try
        {
            Image.Load(path);
        }
        catch (Exception e)
        {
            MessageBoxRequest?.Invoke(this, new MvvmMessageBoxEventArgs(
                null,
                $"ファイルの読み込みに失敗しました\n{e.Message}",
                icon: MessageBoxImage.Error
            ));
            ImagePanelFooterRightMessage = "画像読み込み失敗";
            return;
        }

        ImagePanelFooterRightMessage = "読み込みました";

        RequestProcessImage();
        LoadImageCommandVisibility = Visibility.Hidden;
    }

    private void SaveImage(string type)
    {
        if (Image.ProcessedImage == null)
        {
            ImagePanelFooterRightMessage = "保存する画像がありません";
            return;
        }

        if (type == "New" || _saveImagePath == "" || !Directory.Exists(Path.GetDirectoryName(_saveImagePath)))
        {
            var dialog = new SaveFileDialog
            {
                Title = "画像を選択してください",
                Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif|All Files|*.*"
            };
            if (dialog.ShowDialog() != true)
            {
                ImagePanelFooterRightMessage = "キャンセルしました";
                return;
            }

            _saveImagePath = dialog.FileName;
        }

        try
        {
            Image.Save(_saveImagePath);
        }
        catch (Exception e)
        {
            MessageBoxRequest?.Invoke(this, new MvvmMessageBoxEventArgs(
                null,
                $"ファイルの保存に失敗しました\n{e.Message}",
                icon: MessageBoxImage.Error
            ));
            ImagePanelFooterRightMessage = "画像保存失敗";
            return;
        }

        ImagePanelFooterRightMessage = "保存しました";
    }

    private void RequestProcessImage()
    {
        _actionUpdateDebouncer.Debounce(() => Image.Process(AddedActions));
    }

    private void SaveAll()
    {
        SaveImage("Override");
        SaveActions("Override");
    }
}