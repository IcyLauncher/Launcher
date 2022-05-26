using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.AnimatedVisuals;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.UI;

namespace IcyLauncher.Helpers;

public class UIElementProvider
{
    public static Storyboard Animate(DependencyObject element, string property, int startValue, int endValue, double lenght)
    {
        var keyFrameStart = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), Value = startValue, EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseInOut } };
        var keyFrameEnd = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(lenght)), Value = endValue, EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseInOut } };

        var anim = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
        anim.KeyFrames.Add(keyFrameStart);
        anim.KeyFrames.Add(keyFrameEnd);

        Storyboard.SetTarget(anim, element);
        Storyboard.SetTargetProperty(anim, property);

        var board = new Storyboard();
        board.Children.Add(anim);

        return board;
    }


    public static Grid MainGrid(GridLength[] rowHeight, params UIElement[] children)
    {
        Grid containerGrid = new();

        for (int i = 0; i < rowHeight.Length; i++)
        {
            containerGrid.RowDefinitions.Add(new() { Height = rowHeight[i] });

            children[i].SetValue(Grid.RowProperty, i);
            containerGrid.Children.Add(children[i]);
        }

        return containerGrid;
    }

    public static StackPanel TitleBar(Color primaryColor, out Button backButton)
    {
        var backIcon = new AnimatedIcon() { Source = new AnimatedBackVisualSource(), FallbackIconSource = new SymbolIconSource() { Symbol = Symbol.Back } };
        backButton = new()
        {
            Visibility = Visibility.Collapsed,
            Margin = new(4, 4, 0, 4),
            Width = 32,
            Height = 32,
            Padding = new(0),
            Background = new SolidColorBrush(Colors.Transparent),
            BorderBrush = new SolidColorBrush(Colors.Transparent),
            Content = new Viewbox() { Child = backIcon, Width = 16, Height = 16},
        };
        backButton.PointerEntered += (s, e) => AnimatedIcon.SetState(backIcon, "PointerOver");
        backButton.PointerExited += (s, e) => AnimatedIcon.SetState(backIcon, "Normal");

        Image iconImage = new()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new(10, 0, 10, 0),
            Width = 18,
            Height = 18,
            Source = "Icon.png".AsImage()
        };

        TextBlock titleTextBlock = new()
        {
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 12,
            Foreground = new SolidColorBrush(primaryColor),
            Text = "IcyLauncher",
        };

        StackPanel containerStackPanel = new()
        {
            Orientation = Orientation.Horizontal,
            Height = 48,
            Visibility = Visibility.Collapsed
        };
        containerStackPanel.Children.Add(backButton);
        containerStackPanel.Children.Add(iconImage);
        containerStackPanel.Children.Add(titleTextBlock);

        return containerStackPanel;
    }

    public static Image Icon()
    {
        Image iconImage = new()
        {
            Width = 16,
            Height = 16,
            Margin = new(8, 0, 0, 0),
            Source = "Icon.png".AsImage()
        };

        return iconImage;
    }

    public static NavigationView NavigationView(out Frame contentFrame)
    {
        contentFrame = new();
        NavigationView containerNavigationView = new() { Content = contentFrame };

        containerNavigationView.SetBinding(Microsoft.UI.Xaml.Controls.NavigationView.IsBackEnabledProperty, new Binding() { 
            Source = contentFrame, 
            Path = new PropertyPath("CanGoBack"), 
            Mode = BindingMode.TwoWay });

        containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uE711,\uE71E", Tag = "Home" });
        containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uE065,\uE065", Tag = "Profiles" });
        containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uF593,\uF59D", Tag = "Cosmetics" });
        containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uF135,\uF135", Tag = "Texturepacks" });
        //containerNavigationView.MenuItems.Add(new NavigationViewItem() { Content = "\uF451,\uF455", Tag = "Servers" });

        containerNavigationView.FooterMenuItems.Add(new NavigationViewItem() { Content = "\uE9EE,\uE9F6", Tag = "Help" });
        containerNavigationView.FooterMenuItems.Add(new NavigationViewItem() { Content = "\uEA95,\uEA9E", Tag = "Settings" });

        return containerNavigationView;
    }
}