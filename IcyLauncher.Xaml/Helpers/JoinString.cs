namespace IcyLauncher.Xaml.Helpers;

public class JoinString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        language + string.Join(((string)parameter).Replace("/n", "\n"), (string[])value);

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        ((string)value).Replace(language, "").Split(((string)parameter).Replace("\n", "/n"));
}