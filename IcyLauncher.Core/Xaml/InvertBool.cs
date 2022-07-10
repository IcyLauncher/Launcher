using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.Core.Xaml;

public class InvertBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        !(bool)value;

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        !(bool)value;
}