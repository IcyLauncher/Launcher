using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace IcyLauncher.Helpers.Extentions;

public static class ColorExtnetions
{
    public static SolidColorBrush AsSolid(this Color input) =>
        new(input);
}