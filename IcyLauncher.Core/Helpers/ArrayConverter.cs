using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.Core.Helpers;

public class ArrayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var array = value.ToString()!.Split(",");
        var req = System.Convert.ToInt32(parameter);
        return array[req <= array.Length - 1 ? req : array.Length - 1 ];
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        value;
}