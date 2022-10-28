using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.AnimatedVisuals;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using System.Windows.Input;
using Windows.System;
using Windows.UI;

namespace IcyLauncher.Helpers;

public class UIElementProvider
{
    #region General
    public static Timeline Animate(
        DependencyObject element,
        string property,
        double? startValue,
        double endValue,
        double lenght)
    {
        DoubleAnimation anim = new()
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
    #endregion


    #region Windows/PopUps
    public static Window MainWindow(
        Color lightIconColor,
        Color darkIconColor)
    {
        Grid mainGrid = new();
        Grid titleBar = TitleBar(lightIconColor, darkIconColor);
        NavigationView navigationView = NavigationView();

        navigationView.SetValue(Grid.RowProperty, 1);

        mainGrid.RowDefinitions.Add(new() { Height = new() });
        mainGrid.RowDefinitions.Add(new() { Height = new(1, GridUnitType.Star) });

        mainGrid.Children.Add(titleBar);
        mainGrid.Children.Add(navigationView);

        return new()
        {
            Content = mainGrid,
            Title = "IcyLauncher"
        };
    }

    public static Window LoggerWindow(
        out TextBlock content)
    {
        content = new()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new(4),
            Height = double.NaN,
            Foreground = (SolidColorBrush)Application.Current.Resources["TextSecondary"]
        };
        ScrollViewer container = new()
        {
            Content = content,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Background = (SolidColorBrush)Application.Current.Resources["BackgroundSolid"]
        };

        return new()
        {
            Content = container,
            Title = "IcyLauncher - Logger"
        };
    }


    public static StackPanel FeedbackContainer(
        out RatingControl rating,
        out TextBox content)
    {
        TextBlock title = new()
        {
            Text = "Do you like IcyLauncher?❄️",
            FontSize = 22,
            HorizontalAlignment = HorizontalAlignment.Center,
            Style = (Style)Application.Current.Resources["Title"]
        };
        TextBlock description = new()
        {
            Text = "Feedback helps us to make your IcyLauncher experience better.\nPlease tell us what you like and what we still can improve.",
            FontSize = 16,
            HorizontalAlignment = HorizontalAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Style = (Style)Application.Current.Resources["Content"]
        };

        RatingControl rating_ = new();
        TextBox content_ = new()
        {
            MaxWidth = 400,
            MaxHeight = 200,
            HorizontalAlignment = HorizontalAlignment.Center,
            Height = double.NaN,
            AcceptsReturn = true,
            IsSpellCheckEnabled = true,
            MaxLength = 200,
            Visibility = Visibility.Collapsed,
            PlaceholderText = "Your Feedback...",
            TextWrapping = TextWrapping.Wrap
        };

        rating_.ValueChanged += (s, e) => content_.Visibility = rating_.Value == -1 ? Visibility.Collapsed : Visibility.Visible;
        ScrollViewer.SetVerticalScrollBarVisibility(content_, ScrollBarVisibility.Auto);

        rating = rating_;
        content = content_;

        StackPanel container = new();
        container.Children.Add(title);
        container.Children.Add(description);
        container.Children.Add(new Viewbox() { Child = rating, Height = 50, Margin = new(0, 12, 0, 0) });
        container.Children.Add(content);
        return container;
    }


    public static ContentDialog AboutDialog(
        Version launcherVersion,
        Version apiVersion,
        ICommand secondaryButtonCommand)
    {
        Viewbox icon = Icon(70, 70, new(), ((SolidColorBrush)Application.Current.Resources["AccentLight"]).Color, ((SolidColorBrush)Application.Current.Resources["AccentDark"]).Color);

        TextBlock headerTitle = new()
        {
            Text = "IcyLauncher",
            Style = (Style)Application.Current.Resources["Title"]
        };
        TextBlock headerContent = new()
        {
            Text = "A modern & feature rich MC:BE version switcher and launcher",
            TextWrapping = TextWrapping.Wrap,
            Style = (Style)Application.Current.Resources["Content"]
        };
        StackPanel headerTextContainer = new()
        {
            Margin = new(82, -4, 0, 0),
            Spacing = -7
        };
        headerTextContainer.Children.Add(headerTitle);
        headerTextContainer.Children.Add(headerContent);

        Grid headerContainer = new();
        headerContainer.Children.Add(icon);
        headerContainer.Children.Add(headerTextContainer);


        Rectangle divider1 = new()
        {
            Height = 1,
            Margin = new(0, 7, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Fill = (SolidColorBrush)Application.Current.Resources["TextDisabled"]
        };


        TextBlock appInfo = new()
        {
            Text = "Product:\nLicensing:",
            Style = (Style)Application.Current.Resources["SubpointSecondary"]
        };
        TextBlock appInfoAwn = new()
        {
            Text = $"Full (x{(Computer.Is64 ? "64" : "32")})\nIcySnex Copyright © 2022\nSome rights reserved",
            Style = (Style)Application.Current.Resources["Subpoint"]
        };

        StackPanel appInfoContainer = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 4
        };
        appInfoContainer.Children.Add(appInfo);
        appInfoContainer.Children.Add(appInfoAwn);

        TextBlock appVersion = new()
        {
            Text = "Launcher Version:\nAPI Version:",
            Style = (Style)Application.Current.Resources["SubpointSecondary"]
        };
        TextBlock appVersionAwn = new()
        {
            Text = $"{launcherVersion.TrimZeros()}\n{apiVersion.TrimZeros()}",
            Style = (Style)Application.Current.Resources["Subpoint"]
        };

        StackPanel appVersionContainer = new()
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            Orientation = Orientation.Horizontal,
            Spacing = 4
        };
        appVersionContainer.Children.Add(appVersion);
        appVersionContainer.Children.Add(appVersionAwn);

        Grid appContainer = new();
        appContainer.Children.Add(appVersionContainer);
        appContainer.Children.Add(appInfoContainer);


        Rectangle divider2 = new()
        {
            Height = 1,
            Margin = new(0, 7, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Fill = (SolidColorBrush)Application.Current.Resources["TextDisabled"]
        };


        TextBlock debugUI = new()
        {
            Text = "UI Layer:\nWinAppSDK:",
            Style = (Style)Application.Current.Resources["SubpointSecondary"]
        };
        TextBlock debugUIAwn = new()
        {
            Text = $"WinUI {Computer.WinUIVersion.TrimZeros()}\n{Computer.WindowsAppSDKVersion.TrimZeros()}", /////////////////////////////
            Style = (Style)Application.Current.Resources["Subpoint"]
        };

        StackPanel debugUIContainer = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 4
        };
        debugUIContainer.Children.Add(debugUI);
        debugUIContainer.Children.Add(debugUIAwn);

        TextBlock debugSystem = new()
        {
            Text = "OS Version:\nFramework:",
            Style = (Style)Application.Current.Resources["SubpointSecondary"]
        };
        TextBlock debugSystemAwn = new()
        {
            Text = $"{Computer.OSVersion}\n.net {Computer.RuntimeVersion}",
            Style = (Style)Application.Current.Resources["Subpoint"]
        };

        StackPanel debugSystemContainer = new()
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            Orientation = Orientation.Horizontal,
            Spacing = 4
        };
        debugSystemContainer.Children.Add(debugSystem);
        debugSystemContainer.Children.Add(debugSystemAwn);

        Grid debugContainer = new();
        debugContainer.Children.Add(debugUIContainer);
        debugContainer.Children.Add(debugSystemContainer);


        StackPanel container = new()
        {
            Spacing = 8
        };
        container.Children.Add(headerContainer);
        container.Children.Add(divider1);
        container.Children.Add(appContainer);
        container.Children.Add(divider2);
        container.Children.Add(debugContainer);

        ContentDialog result = new()
        {
            Content = container,
            CloseButtonText = "Close",
            PrimaryButtonText = "License",
            SecondaryButtonText = "Send Feedback",
            SecondaryButtonCommand = secondaryButtonCommand,
        };
        result.PrimaryButtonClick += async (s, e) =>
            await Launcher.LaunchUriAsync(new("https://raw.githubusercontent.com/IcyLauncher/Additional/main/LICENSE"));

        return result;
    }
    #endregion


    #region TitleBar
    public static Grid TitleBar(
        Color lightIconColor,
        Color darkIconColor)
    {
        AnimatedIcon backIcon = new()
        {
            Source = new AnimatedBackVisualSource(),
            FallbackIconSource = new SymbolIconSource()
            {
                Symbol = Symbol.Back
            }
        };
        Button backButton = new()
        {
            Margin = new(4, 4, 0, 4),
            Width = 0,
            Height = 32,
            Padding = new(0),
            Background = Colors.Transparent.AsSolid(),
            BorderBrush = Colors.Transparent.AsSolid(),
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

        StackPanel titleBarContent = new()
        {
            Orientation = Orientation.Horizontal,
            Height = 48
        };
        titleBarContent.Children.Add(backButton);
        titleBarContent.Children.Add(Icon(19, 19, new(10, 0, 10, 0), lightIconColor, darkIconColor));
        titleBarContent.Children.Add(titleTextBlock);

        Grid dragArea = new()
        {
            Margin = new(39, 0, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        Grid container = new()
        {
            Visibility = Visibility.Collapsed
        };
        container.Children.Add(titleBarContent);
        container.Children.Add(dragArea);

        return container;
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

        return new()
        {
            Width = width,
            Height = height,
            Margin = margin,
            Child = path,
            HorizontalAlignment = HorizontalAlignment.Left
        };
    }

    public static readonly string IconPath = "M31,3.53v11.39v10.33h0c0,0.49-0.12,0.98-0.39,1.43c-0.79,1.35-2.55,1.81-3.91,1.02c0,0,0,0-0.01,0l0,0  L9.94,18.06l-2.23-1.28c-1.58-0.91-1.95-3.08-0.58-4.46c0.91-0.92,2.34-1.07,3.46-0.43l13.59,7.83v0c0.1,0.05,0.22,0.08,0.34,0.08  c0.4,0,0.73-0.32,0.74-0.72h0V3.59c0-1.36,0.94-2.57,2.28-2.83C29.39,0.4,31,1.78,31,3.53z M33.22,3.53v21.72h0  c0,0.49,0.12,0.98,0.39,1.43c0.79,1.35,2.55,1.81,3.91,1.02c0,0,0,0,0.01,0l0,0L56.5,16.77c1.58-0.91,1.95-3.08,0.58-4.46  c-0.91-0.92-2.34-1.07-3.46-0.43l-13.59,7.83v0c-0.1,0.05-0.22,0.08-0.34,0.08c-0.4,0-0.73-0.32-0.74-0.72h0V3.59  c0-1.36-0.94-2.57-2.28-2.83C34.83,0.4,33.22,1.78,33.22,3.53z M57.07,18.64L38.26,29.5l0,0c-0.42,0.24-0.79,0.6-1.05,1.05  c-0.77,1.36-0.29,3.11,1.07,3.9c0,0,0,0,0.01,0l0,0l18.95,10.96c1.57,0.91,3.64,0.15,4.15-1.73c0.34-1.25-0.24-2.56-1.36-3.21  l-13.57-7.85l0,0c-0.1-0.06-0.18-0.15-0.24-0.25c-0.2-0.35-0.09-0.79,0.25-1l0,0l13.41-7.74c1.18-0.68,1.76-2.1,1.32-3.39  C60.58,18.47,58.59,17.77,57.07,18.64z M56.04,46.94L37.22,36.08l0,0c-0.42-0.24-0.91-0.38-1.43-0.38c-1.57,0.01-2.84,1.3-2.84,2.88  c0,0,0,0,0,0.01l0,0l-0.02,21.89c0,1.82,1.69,3.23,3.58,2.73c1.25-0.33,2.1-1.49,2.1-2.79l0.01-15.68l0,0  c0.01-0.11,0.04-0.23,0.1-0.33c0.2-0.35,0.64-0.47,0.99-0.28l0,0l13.41,7.74c1.18,0.68,2.7,0.47,3.59-0.55  C57.94,49.9,57.55,47.81,56.04,46.94z M30.9,60.37V38.65h0c0-0.49-0.12-0.98-0.39-1.43c-0.79-1.35-2.55-1.81-3.91-1.02  c0,0,0,0-0.01,0l0,0L7.63,47.13c-1.58,0.91-1.95,3.08-0.58,4.46c0.91,0.92,2.34,1.07,3.46,0.43l13.59-7.83v0  c0.1-0.05,0.22-0.08,0.34-0.08c0.4,0,0.73,0.32,0.74,0.72h0v15.48c0,1.36,0.94,2.57,2.28,2.83C29.29,63.51,30.9,62.12,30.9,60.37z   M6.93,45.27l18.81-10.86l0,0c0.42-0.24,0.79-0.6,1.05-1.05c0.77-1.36,0.29-3.11-1.07-3.9c0,0,0,0-0.01,0l0,0L6.76,18.49  c-1.57-0.91-3.64-0.15-4.15,1.73c-0.34,1.25,0.24,2.56,1.36,3.21l13.57,7.85l0,0c0.1,0.06,0.18,0.15,0.24,0.25  c0.2,0.35,0.09,0.79-0.25,1l0,0L4.12,40.28c-1.18,0.68-1.76,2.1-1.32,3.39C3.42,45.44,5.41,46.15,6.93,45.27z";
    #endregion

    #region NavigationView
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
    #endregion
}