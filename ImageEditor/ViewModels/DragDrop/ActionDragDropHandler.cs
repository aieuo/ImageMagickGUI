using System.IO;
using System.Windows;
using GongSolutions.Wpf.DragDrop;

namespace ImageEditor.ViewModels.DragDrop;

public class ActionDragDropHandler(Action<string> onDrop) : DefaultDropHandler
{
    public override void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not DataObject dataObject)
        {
            base.DragOver(dropInfo);
            return;
        }
        
        var dragFileList = dataObject.GetFileDropList().Cast<string>();
        dropInfo.Effects = dragFileList.Any(IsJson) ? DragDropEffects.Copy : DragDropEffects.None;
    }

    public override void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not DataObject dataObject)
        {
            base.Drop(dropInfo);
            return;
        }
        
        var dragFileList = dataObject.GetFileDropList().Cast<string>().ToList();
        dropInfo.Effects = dragFileList.Any(IsJson) ? DragDropEffects.Copy : DragDropEffects.None;

        var file = dragFileList.Where(IsJson).FirstOrDefault();
        if (file == null)
        {
            return;
        }

        onDrop(file);
    }

    private static bool IsJson(string data)
    {
        var extension = Path.GetExtension(data);
        return extension is ".json";
    }
}