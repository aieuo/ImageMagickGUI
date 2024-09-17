using System.Windows;
using GongSolutions.Wpf.DragDrop;

namespace ImageEditor.ViewModels.DragDrop;

public class ImageDragDropHandler(Action<string> onDrop) : IDropTarget
{
    public void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not DataObject)
        {
            dropInfo.Effects = DragDropEffects.None;
            return;
        }

        dropInfo.Effects = DragDropEffects.Copy;
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not DataObject dataObject)
        {
            dropInfo.Effects = DragDropEffects.None;
            return;
        }

        var dragFileList = dataObject.GetFileDropList().Cast<string>();
        dropInfo.Effects = DragDropEffects.Copy;

        var file = dragFileList.FirstOrDefault();
        if (file == null)
        {
            return;
        }

        onDrop(file);
    }
}