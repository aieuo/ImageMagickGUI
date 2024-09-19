using CommunityToolkit.Mvvm.ComponentModel;
using ImageEditor.Utils;
using ImageMagick;

namespace ImageEditor.Models;

public partial class Image : ObservableObject
{
    [ObservableProperty]
    private MagickImage? _originalImage = null;
    
    [ObservableProperty]
    private MagickImage? _processedImage = null;

    [ObservableProperty]
    private bool _isProcessingImage = false;

    private bool _shouldReProcessImage = false;
    
    public void Load(string path)
    {
        OriginalImage = new MagickImage(path);
        OriginalImage.ColorSpace = ColorSpace.sRGB;
        
        ProcessedImage = null;
    }
    
    public void Save(string path)
    {
        if (ProcessedImage == null)
        {
            throw new NullReferenceException("ProcessedImage is null");
        }

        ProcessedImage.Write(path);
    }

    public async void Process(ICollection<Actions.Action> actions)
    {
        if (OriginalImage == null) return;

        if (IsProcessingImage)
        {
            _shouldReProcessImage = true;
            return;
        }

        IsProcessingImage = true;

        await Task.Run(() =>
        {
            var tmp = (MagickImage)OriginalImage.Clone();
            foreach (var action in actions)
            {
                action.ProcessImage(tmp);
            }

            ProcessedImage = tmp;
        });

        IsProcessingImage = false;

        if (_shouldReProcessImage)
        {
            _shouldReProcessImage = false;
            Process(actions);
        }
    }
}