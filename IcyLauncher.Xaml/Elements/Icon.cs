using Microsoft.UI;
using Windows.UI;

namespace IcyLauncher.Xaml.Elements;

public class Icon : ContentControl
{
    public static readonly DependencyProperty LightColorProperty = DependencyProperty.Register(
        "LightColor", typeof(Color), typeof(Icon), new(Colors.White));

    public Color LightColor
    {
        get => (Color)GetValue(LightColorProperty);
        set => SetValue(LightColorProperty, value);
    }

    public static readonly DependencyProperty DarkColorProperty = DependencyProperty.Register(
        "DarkColor", typeof(Color), typeof(Icon), new(Colors.Black));

    public Color DarkColor
    {
        get => (Color)GetValue(DarkColorProperty);
        set => SetValue(DarkColorProperty, value);
    }


    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }
}