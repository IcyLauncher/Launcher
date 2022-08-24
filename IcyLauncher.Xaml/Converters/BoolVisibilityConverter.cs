namespace IcyLauncher.Xaml.Converters;

public class BoolVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        (string)parameter == "1" ?
        (bool)value ? Visibility.Visible : Visibility.Collapsed :
        (bool)value ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        (string)parameter == "0" ?
        (Visibility)value == Visibility.Visible :
        (Visibility)value == Visibility.Collapsed;
}