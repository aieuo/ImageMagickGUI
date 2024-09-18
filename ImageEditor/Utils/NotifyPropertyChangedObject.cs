using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ImageEditor.Utils;

public abstract class NotifyPropertyChangedObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T target, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(target, value)) return false;
            
        target = value;
        NotifyPropertyChanged(propertyName);
        return true;
    }
}