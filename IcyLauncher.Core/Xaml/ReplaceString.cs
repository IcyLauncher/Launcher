using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.Core.Xaml;

public class ReplaceString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        ((string)value).Replace((string)parameter, language);

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        value;
}