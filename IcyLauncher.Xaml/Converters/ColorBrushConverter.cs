using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace IcyLauncher.Xaml.Converters;

public class ColorBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        ((Color)value).AsSolid();

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        ((SolidColorBrush)value).Color;
}