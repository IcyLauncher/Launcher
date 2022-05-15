namespace IcyLauncher.Extentions;

static class StringExtentions
{
    public static string FromAssets(this string input) =>
        $"ms-appx:///Assets/{input}";
}