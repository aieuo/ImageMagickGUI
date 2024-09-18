using ImageEditor.Utils;
using ImageMagick;

namespace ImageEditor.Models;

public class Image : NotifyPropertyChangedObject
{
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
}