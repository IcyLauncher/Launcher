namespace IcyLauncher.Xaml.Converters;

public class BoolScrollbarVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        (bool)value ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden;

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        (ScrollBarVisibility)value == ScrollBarVisibility.Auto;
}