using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace IcyLauncher.Xaml.Elements;

public class MiniLauncher : ContentControl
{
    #region General
    #region BannerBrush
    public static readonly DependencyProperty BannerBrushProperty = DependencyProperty.Register(
        "BannerBrush", typeof(Brush), typeof(MiniLauncher), new(Colors.White.AsSolid()));

    public Brush BannerBrush
    {
        get => (Brush)GetValue(BannerBrushProperty);
        set => SetValue(BannerBrushProperty, value);
    }
    #endregion

    #region BackgroundSolid
    public static readonly DependencyProperty BackgroundSolidProperty = DependencyProperty.Register(
        "BackgroundSolid", typeof(SolidColorBrush), typeof(MiniLauncher), new(Colors.White.AsSolid()));

    public SolidColorBrush BackgroundSolid
    {
        get => (SolidColorBrush)GetValue(BackgroundSolidProperty);
        set => SetValue(BackgroundSolidProperty, value);
    }
    #endregion

    #region AccentPrimary
    public static readonly DependencyProperty AccentPrimaryPorperty = DependencyProperty.Register(
        "AccentPrimary", typeof(SolidColorBrush), typeof(MiniLauncher), new(Colors.White.AsSolid()));

    public SolidColorBrush AccentPrimary
    {
        get => (SolidColorBrush)GetValue(AccentPrimaryPorperty);
        set => SetValue(AccentPrimaryPorperty, value);
    }
    #endregion

    #region ControlPrimary
    public static readonly DependencyProperty ControlPrimaryProperty = DependencyProperty.Register(
        "ControlPrimary", typeof(SolidColorBrush), typeof(MiniLauncher), new(Colors.White.AsSolid()));

    public SolidColorBrush ControlPrimary
    {
        get => (SolidColorBrush)GetValue(ControlPrimaryProperty);
        set => SetValue(ControlPrimaryProperty, value);
    }
    #endregion

    #region BackgroundGradient
    public static readonly DependencyProperty BackgroundGradientProperty = DependencyProperty.Register(
        "BackgroundGradient", typeof(SolidColorBrush), typeof(MiniLauncher), new(Colors.White.AsSolid()));

    public SolidColorBrush BackgroundGradient
    {
        get => (SolidColorBrush)GetValue(BackgroundGradientProperty);
        set => SetValue(BackgroundGradientProperty, value);
    }
    #endregion

    #region BackgroundGradientTransparent
    public static readonly DependencyProperty BackgroundGradientTransparentProperty = DependencyProperty.Register(
        "BackgroundGradientTransparent", typeof(SolidColorBrush), typeof(MiniLauncher), new(Colors.White.AsSolid()));

    public SolidColorBrush BackgroundGradientTransparent
    {
        get => (SolidColorBrush)GetValue(BackgroundGradientTransparentProperty);
        set => SetValue(BackgroundGradientTransparentProperty, value);
    }
    #endregion

    #region TextSecondary
    public static readonly DependencyProperty TextSecondaryProperty = DependencyProperty.Register(
        "TextSecondary", typeof(SolidColorBrush), typeof(MiniLauncher), new(Colors.White.AsSolid()));

    public SolidColorBrush TextSecondary
    {
        get => (SolidColorBrush)GetValue(TextSecondaryProperty);
        set => SetValue(TextSecondaryProperty, value);
    }
    #endregion

    #endregion
}