using Microsoft.UI.Xaml.Media.Imaging;
using System.Reflection;

namespace IcyLauncher.Core.Extentions;

public static class StringExtentions
{
    public static string FromAssets(this string input) =>
        $"ms-appx:///Assets/{input}";

    public static BitmapImage AsImage(this string input) =>
        new(new($"ms-appx:///Assets/{input}"));

    public static Type? AsType(this string input) =>
        Assembly.LoadFile($"{Computer.CurrentDirectory}\\IcyLauncher.dll").GetType(input);
}