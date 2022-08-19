using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace IcyLauncher.Core.Xaml;

public class ValidateNo255 : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var col = (Color)value;
        if (col.R == 255 && col.G == 255 && col.B == 255)
            return new SolidColorBrush(Color.FromArgb(col.A, 253, 253, 253));
        return new SolidColorBrush(col);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        ((SolidColorBrush)value).Color;
}