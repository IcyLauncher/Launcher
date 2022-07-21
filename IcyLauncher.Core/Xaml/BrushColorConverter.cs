using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace IcyLauncher.Core.Xaml;

public class BrushColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        new SolidColorBrush((Color)value);

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        ((SolidColorBrush)value).Color;
}