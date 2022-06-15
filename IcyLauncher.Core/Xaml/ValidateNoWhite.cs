using Microsoft.UI.Xaml.Data;
using Windows.UI;

namespace IcyLauncher.Core.Xaml;

public class ValidateNoWhite : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var col = (Color)value;
        if (col.R == 255 && col.G == 255 && col.B == 255)
            return Color.FromArgb(col.A, 254, 254, 254);
        return col;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        value;
}