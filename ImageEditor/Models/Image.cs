using ImageEditor.Utils;
using ImageMagick;

namespace ImageEditor.Models;

public class Image : NotifyPropertyChangedObject
{
    #region Properties
    
    private MagickImage? _originalImage = null;

    public MagickImage? OriginalImage
    {
        get => _originalImage;
        set => SetProperty(ref _originalImage, value);
    }

    private MagickImage? _processedImage = null;

    public MagickImage? ProcessedImage
    {
        get => _processedImage;
        set => SetProperty(ref _processedImage, value);
    }
    
    private bool _isProcessingImage = false;

    public bool IsProcessingImage
    {
        get => _isProcessingImage;
        set => SetProperty(ref _isProcessingImage, value);
    }
    
    #endregion

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

        if (_isProcessingImage)
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