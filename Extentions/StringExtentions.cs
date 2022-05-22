using Microsoft.UI.Xaml.Media.Imaging;

namespace IcyLauncher.Extentions;

static class StringExtentions
{
    public static string FromAssets(this string input) =>
        $"ms-appx:///Assets/{input}";

    public static BitmapImage AsImage(this string input) =>
        new(new($"ms-appx:///Assets/{input}"));
}