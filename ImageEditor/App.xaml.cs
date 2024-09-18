using System.Configuration;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using ImageEditor.ViewModels;
using ImageMagick;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;

namespace ImageEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IDialogService, DialogService>()
                    .AddTransient<MainWindowViewModel>()
                    .BuildServiceProvider());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MagickNET.Initialize();
        }
    }
}