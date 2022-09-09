﻿using Microsoft.UI.Xaml.Media.Imaging;
using System.Text;

namespace IcyLauncher.Helpers.Extentions;

public static class StringExtentions
{
    public static Uri FromAssets(this string input) =>
        new($"ms-appx:///Assets/{input}");

    public static BitmapImage AsImage(this string input, bool fromAssets = true) =>
        new(fromAssets ? input.FromAssets() : new(input));

    public static Type? AsType(this string input, string assembly = "IcyLauncher.WinUI") =>
        Type.GetType($"{assembly}.{input}, {assembly}");

    public static byte[] AsBytes(this string input) =>
        Encoding.ASCII.GetBytes(input);
}