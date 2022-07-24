using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace IcyLauncher.UI;

public class MiniLauncher : ContentControl
{
    public Brush BannerBrush
    {
        get => (Brush)GetValue(BannerBrushProperty);
        set => SetValue(BannerBrushProperty, value);
    }

    public static readonly DependencyProperty BannerBrushProperty = DependencyProperty.Register(
        "BannerBrush",
        typeof(Brush),
        typeof(MiniLauncher),
        new PropertyMetadata(new SolidColorBrush(Colors.White)));
}