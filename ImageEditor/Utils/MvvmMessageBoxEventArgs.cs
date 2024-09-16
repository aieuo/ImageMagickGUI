using System.Windows;

namespace ImageEditor.Utils;

// https://zenn.dev/ryokusasa/articles/37461307de2908
public class MvvmMessageBoxEventArgs(
    Action<MessageBoxResult>? resultAction,
    string messageBoxText,
    string caption = "",
    MessageBoxButton button = MessageBoxButton.OK,
    MessageBoxImage icon = MessageBoxImage.None,
    MessageBoxResult defaultResult = MessageBoxResult.None,
    MessageBoxOptions options = MessageBoxOptions.None)
    : EventArgs
{
    public void Show(Window owner)
    {
        var messageBoxResult = MessageBox.Show(owner, messageBoxText, caption, button, icon, defaultResult, options);
        resultAction?.Invoke(messageBoxResult);
    }

    public void Show()
    {
        var messageBoxResult = MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options);
        resultAction?.Invoke(messageBoxResult);
    }
}