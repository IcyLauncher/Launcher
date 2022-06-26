using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.Core.Xaml;

public class ArrayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var array = value.ToString()!.Split(",");
        var requested = System.Convert.ToInt32(parameter);

        return array[requested <= array.Length - 1 ? requested : array.Length - 1 ];
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        string.Join(",", (object[])value);
}