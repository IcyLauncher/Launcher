using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.AnimatedVisuals;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace IcyLauncher.Helpers;

public class UIElementProvider
{
    public static Timeline Animate(
        DependencyObject element,
        string property,
        double? startValue,
        double endValue,
        double lenght)
    {
        var anim = new DoubleAnimation()
        {
            Duration = TimeSpan.FromMilliseconds(lenght),
            From = startValue,
            To = endValue,
            EnableDependentAnimation = true
        };

        Storyboard.SetTarget(anim, element);
        Storyboard.SetTargetProperty(anim, property);

        return anim;
    }


    public static Grid MainGrid(
        GridLength[] rowHeight,
        params UIElement[] children)
    {
        Grid mainGrid = new();

        for (int i = 0; i < rowHeight.Length; i++)
        {
            mainGrid.RowDefinitions.Add(new() { Height = rowHeight[i] });

            children[i].SetValue(Grid.RowProperty, i);
            mainGrid.Children.Add(children[i]);
        }

        return mainGrid;
    }


    public static StackPanel TitleBar(
        Color lightIconColor,
        Color darkIconColor)
    {
        AnimatedIcon backIcon = new() { Source = new AnimatedBackVisualSource(), FallbackIconSource = new SymbolIconSource() { Symbol = Symbol.Back } };
        Button backButton = new()
        {
            Margin = new(4, 4, 0, 4),
            Width = 0,
            Height = 32,
            Padding = new(0),
            Background = new SolidColorBrush(Colors.Transparent),
            BorderBrush = new SolidColorBrush(Colors.Transparent),
            Content = new Viewbox() { Child = backIcon, Width = 16, Height = 16 },
            Opacity = 0,
            OpacityTransition = new()
        };
        backButton.PointerEntered += (s, e) => AnimatedIcon.SetState(backIcon, "PointerOver");
        backButton.PointerExited += (s, e) => AnimatedIcon.SetState(backIcon, "Normal");

        TextBlock titleTextBlock = new()
        {
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 12,
            Text = "IcyLauncher",
        };

        StackPanel containerStackPanel = new()
        {
            Orientation = Orientation.Horizontal,
            Height = 48,
            Visibility = Visibility.Collapsed
        };
        containerStackPanel.Children.Add(backButton);
        containerStackPanel.Children.Add(Icon(19, 19, new(10, 0, 10, 0), lightIconColor, darkIconColor));
        containerStackPanel.Children.Add(titleTextBlock);

        return containerStackPanel;
    }

    public static Viewbox Icon(
        int width,
        int height,
        Thickness margin,
        Color lightColor,
        Color darkColor)
    {
        GradientStopCollection colorCollection = new();
        colorCollection.Add(new() { Color = lightColor, Offset = 0.1 });
        colorCollection.Add(new() { Color = darkColor, Offset = 1 });

        Path path = new() { Fill = new LinearGradientBrush() { GradientStops = colorCollection, StartPoint = new(0, 0), EndPoint = new(0.625, 1) } };
        path.SetBinding(Path.DataProperty, new Binding() { Source = IconPath });

        return new() { Width = width, Height = height, Margin = margin, Child = path };
    }

    public static readonly string IconPath = "M31,3.53v11.39v10.33h0c0,0.49-0.12,0.98-0.39,1.43c-0.79,1.35-2.55,1.81-3.91,1.02c0,0,0,0-0.01,0l0,0  L9.94,18.06l-2.23-1.28c-1.58-0.91-1.95-3.08-0.58-4.46c0.91-0.92,2.34-1.07,3.46-0.43l13.59,7.83v0c0.1,0.05,0.22,0.08,0.34,0.08  c0.4,0,0.73-0.32,0.74-0.72h0V3.59c0-1.36,0.94-2.57,2.28-2.83C29.39,0.4,31,1.78,31,3.53z M33.22,3.53v21.72h0  c0,0.49,0.12,0.98,0.39,1.43c0.79,1.35,2.55,1.81,3.91,1.02c0,0,0,0,0.01,0l0,0L56.5,16.77c1.58-0.91,1.95-3.08,0.58-4.46  c-0.91-0.92-2.34-1.07-3.46-0.43l-13.59,7.83v0c-0.1,0.05-0.22,0.08-0.34,0.08c-0.4,0-0.73-0.32-0.74-0.72h0V3.59  c0-1.36-0.94-2.57-2.28-2.83C34.83,0.4,33.22,1.78,33.22,3.53z M57.07,18.64L38.26,29.5l0,0c-0.42,0.24-0.79,0.6-1.05,1.05  c-0.77,1.36-0.29,3.11,1.07,3.9c0,0,0,0,0.01,0l0,0l18.95,10.96c1.57,0.91,3.64,0.15,4.15-1.73c0.34-1.25-0.24-2.56-1.36-3.21  l-13.57-7.85l0,0c-0.1-0.06-0.18-0.15-0.24-0.25c-0.2-0.35-0.09-0.79,0.25-1l0,0l13.41-7.74c1.18-0.68,1.76-2.1,1.32-3.39  C60.58,18.47,58.59,17.77,57.07,18.64z M56.04,46.94L37.22,36.08l0,0c-0.42-0.24-0.91-0.38-1.43-0.38c-1.57,0.01-2.84,1.3-2.84,2.88  c0,0,0,0,0,0.01l0,0l-0.02,21.89c0,1.82,1.69,3.23,3.58,2.73c1.25-0.33,2.1-1.49,2.1-2.79l0.01-15.68l0,0  c0.01-0.11,0.04-0.23,0.1-0.33c0.2-0.35,0.64-0.47,0.99-0.28l0,0l13.41,7.74c1.18,0.68,2.7,0.47,3.59-0.55  C57.94,49.9,57.55,47.81,56.04,46.94z M30.9,60.37V38.65h0c0-0.49-0.12-0.98-0.39-1.43c-0.79-1.35-2.55-1.81-3.91-1.02  c0,0,0,0-0.01,0l0,0L7.63,47.13c-1.58,0.91-1.95,3.08-0.58,4.46c0.91,0.92,2.34,1.07,3.46,0.43l13.59-7.83v0  c0.1-0.05,0.22-0.08,0.34-0.08c0.4,0,0.73,0.32,0.74,0.72h0v15.48c0,1.36,0.94,2.57,2.28,2.83C29.29,63.51,30.9,62.12,30.9,60.37z   M6.93,45.27l18.81-10.86l0,0c0.42-0.24,0.79-0.6,1.05-1.05c0.77-1.36,0.29-3.11-1.07-3.9c0,0,0,0-0.01,0l0,0L6.76,18.49  c-1.57-0.91-3.64-0.15-4.15,1.73c-0.34,1.25,0.24,2.56,1.36,3.21l13.57,7.85l0,0c0.1,0.06,0.18,0.15,0.24,0.25  c0.2,0.35,0.09,0.79-0.25,1l0,0L4.12,40.28c-1.18,0.68-1.76,2.1-1.32,3.39C3.42,45.44,5.41,46.15,6.93,45.27z";


    public static NavigationView NavigationView()
    {
        Frame contentFrame = new();
        NavigationView containerNavigationView = new() { Content = contentFrame };

        containerNavigationView.SetBinding(Microsoft.UI.Xaml.Controls.NavigationView.IsBackEnabledProperty, new Binding()
        {
            Source = contentFrame,
            Path = new PropertyPath("CanGoBack"),
            Mode = BindingMode.TwoWay
        });

        containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uE711|\uE71E", Tag = "Home" });
        containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uE065|\uE065", Tag = "Profiles" });
        containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uF593|\uF59D", Tag = "Cosmetics" });
        containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uF135|\uF135", Tag = "Texturepacks" });
        //containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uF451|\uF455", Tag = "Servers" });

        containerNavigationView.FooterMenuItems.Add(new NavigationViewItem() { Content = "\uE9EE|\uE9F6", Tag = "Help" });
        containerNavigationView.FooterMenuItems.Add(new NavigationViewItem() { Content = "\uEA95|\uEA9E", Tag = "Settings" });

        return containerNavigationView;
    }
}