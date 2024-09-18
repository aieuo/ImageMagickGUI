using CommunityToolkit.Mvvm.DependencyInjection;
using ImageEditor.ViewModels;

namespace ImageEditor;

public class ViewModelLocator
{
    public MainWindowViewModel MainWindow => Ioc.Default.GetRequiredService<MainWindowViewModel>();
}