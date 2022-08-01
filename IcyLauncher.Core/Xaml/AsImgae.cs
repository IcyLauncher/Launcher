using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System.IO;

namespace IcyLauncher.Core.Xaml;

public class AsImage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        Path.Combine(Computer.CurrentDirectory, (string)value).AsImage(false);

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        ((BitmapImage)value).UriSource.AbsolutePath;
}