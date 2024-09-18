using System.Globalization;
using System.Windows.Data;
using ImageMagick;

namespace ImageEditor.Views.Converters;

public class MagickImageToImageSourceConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MagickImage image)
        {
            return image.ToBitmapSource();
        }
        
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}