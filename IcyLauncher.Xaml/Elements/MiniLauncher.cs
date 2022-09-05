using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace IcyLauncher.Xaml.Elements;

public class MiniLauncher : ContentControl
{
    public static readonly DependencyProperty BannerBrushProperty = DependencyProperty.Register(
        "BannerBrush", typeof(Brush), typeof(MiniLauncher), new(new SolidColorBrush(Colors.White)));

    public Brush BannerBrush
    {
        get => (Brush)GetValue(BannerBrushProperty);
        set => SetValue(BannerBrushProperty, value);
    }


    public static readonly DependencyProperty BackgroundSolidProperty = DependencyProperty.Register(
        "BackgroundSolid", typeof(SolidColorBrush), typeof(MiniLauncher), new(new SolidColorBrush(Colors.White)));

    public SolidColorBrush BackgroundSolid
    {
        get => (SolidColorBrush)GetValue(BackgroundSolidProperty);
        set => SetValue(BackgroundSolidProperty, value);
    }


    public static readonly DependencyProperty AccentPrimaryPorperty = DependencyProperty.Register(
        "AccentPrimary", typeof(SolidColorBrush), typeof(MiniLauncher), new(new SolidColorBrush(Colors.White)));

    public SolidColorBrush AccentPrimary
    {
        get => (SolidColorBrush)GetValue(AccentPrimaryPorperty);
        set => SetValue(AccentPrimaryPorperty, value);
    }


    public static readonly DependencyProperty ControlPrimaryProperty = DependencyProperty.Register(
        "ControlPrimary", typeof(SolidColorBrush), typeof(MiniLauncher), new(new SolidColorBrush(Colors.White)));

    public SolidColorBrush ControlPrimary
    {
        get => (SolidColorBrush)GetValue(ControlPrimaryProperty);
        set => SetValue(ControlPrimaryProperty, value);
    }


    public static readonly DependencyProperty BackgroundGradientTransparentProperty = DependencyProperty.Register(
        "BackgroundGradientTransparent", typeof(SolidColorBrush), typeof(MiniLauncher), new(new SolidColorBrush(Colors.White)));

    public SolidColorBrush BackgroundGradientTransparent
    {
        get => (SolidColorBrush)GetValue(BackgroundGradientTransparentProperty);
        set => SetValue(BackgroundGradientTransparentProperty, value);
    }


    public static readonly DependencyProperty BackgroundGradientProperty = DependencyProperty.Register(
        "BackgroundGradient", typeof(SolidColorBrush), typeof(MiniLauncher), new(new SolidColorBrush(Colors.White)));

    public SolidColorBrush BackgroundGradient
    {
        get => (SolidColorBrush)GetValue(BackgroundGradientProperty);
        set => SetValue(BackgroundGradientProperty, value);
    }


    public static readonly DependencyProperty TextSecondaryProperty = DependencyProperty.Register(
        "TextSecondary", typeof(SolidColorBrush), typeof(MiniLauncher), new(new SolidColorBrush(Colors.White)));

    public SolidColorBrush TextSecondary
    {
        get => (SolidColorBrush)GetValue(TextSecondaryProperty);
        set => SetValue(TextSecondaryProperty, value);
    }
}