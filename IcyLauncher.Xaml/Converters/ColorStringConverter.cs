using Microsoft.UI;
using Microsoft.UI.Xaml.Markup;
using Windows.UI;

namespace IcyLauncher.Xaml.Converters;

public class ColorStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        ((Color)value).ToString();

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        try
        {
            return XamlBindingHelper.ConvertValue(typeof(Color), value);
        }
        catch
        {
            return Colors.Black;
        }
    }
}