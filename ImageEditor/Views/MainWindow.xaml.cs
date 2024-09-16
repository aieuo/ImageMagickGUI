﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageEditor.Utils;
using ImageEditor.ViewModels;

namespace ImageEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.DeleteActionDialogRequest += ShowConfirmDeleteDialog;
            }
        }
        
        private void ShowConfirmDeleteDialog(object? sender, MvvmMessageBoxEventArgs args)
        {
            args.Show();
        }
    }
}