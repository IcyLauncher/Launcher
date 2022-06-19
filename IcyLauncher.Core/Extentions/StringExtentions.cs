using Microsoft.UI.Xaml.Media.Imaging;
using System.Text;

namespace IcyLauncher.Core.Extentions;

public static class StringExtentions
{
    public static string FromAssets(this string input) =>
        $"ms-appx:///Assets/{input}";

    public static BitmapImage AsImage(this string input) =>
        new(new(input.FromAssets()));

    public static Type? AsType(this string input, string assembly = "IcyLauncher") =>
        Type.GetType($"{assembly}.{input}, {assembly}");

    public static byte[] AsBytes(this string input) =>
        Encoding.ASCII.GetBytes(input);
}