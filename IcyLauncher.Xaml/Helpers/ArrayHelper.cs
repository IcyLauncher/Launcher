namespace IcyLauncher.Xaml.Helpers;

public class ArrayHelper : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        string[]? array = value.ToString()!.Split("|");
        int requested = System.Convert.ToInt32(parameter);

        return array[requested <= array.Length - 1 ? requested : array.Length - 1];
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        string.Join("|", (object[])value);
}