using IcyLauncher.Helpers;
using Microsoft.UI.Xaml.Media.Imaging;
using System.IO;

namespace IcyLauncher.Xaml.Helpers;

public class AsImage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        System.Convert.ToInt32(parameter) == 0 ? Path.Combine(Computer.CurrentDirectory, (string)value).AsImage(false) : ((string)value).AsImage(false);

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        ((BitmapImage)value).UriSource.AbsolutePath;
}