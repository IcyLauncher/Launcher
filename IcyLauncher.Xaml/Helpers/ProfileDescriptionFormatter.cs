using IcyLauncher.Data.Models;

namespace IcyLauncher.Xaml.Helpers;

public class ProfileDescriptionFormatter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        value is Profile profile ? $"Version {profile.Version} | {profile.Client}" : parameter;

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        value;
}