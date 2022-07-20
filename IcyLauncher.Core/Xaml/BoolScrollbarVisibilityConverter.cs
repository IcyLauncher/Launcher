using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.Core.Xaml;

public class BoolScrollbarVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        (bool)value ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden;

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        (ScrollBarVisibility)value == ScrollBarVisibility.Auto;
}