using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using Windows.UI;

namespace IcyLauncher.Helpers;

public class UIElementProvider
{
    public static Grid MainGrid(GridLength[] rowHeight, params UIElement[] children)
    {
        Grid grid = new();

        for (int i = 0; i < rowHeight.Length; i++)
        {
            grid.RowDefinitions.Add(new() { Height = rowHeight[i] });

            children[i].SetValue(Grid.RowProperty, i);
            grid.Children.Add(children[i]);
        }

        return grid;
    }

    public static StackPanel TitleBar(Color primaryColor)
    {
        Button button = new()
        {
            Margin = new(4, 4, 0, 4),
            Width = 32,
            Height = 32,
            Padding = new(0),
            Background = new SolidColorBrush(Colors.Transparent),
            BorderBrush = new SolidColorBrush(Colors.Transparent),
            Content = new Viewbox() { Child = new SymbolIcon(Symbol.Back) { Foreground = new SolidColorBrush(Colors.LightGray) }, Width = 11, Height = 11},
        };

        Image image = new()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Width = 18,
            Height = 18,
            Margin = new(16, 0, 16, 0),
            Source = "Icon.png".AsImage()
        };

        TextBlock textBlock = new()
        {
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 12,
            Foreground = new SolidColorBrush(primaryColor),
            Text = "IcyLauncher",
        };

        StackPanel stackPanel = new()
        {
            Orientation = Orientation.Horizontal,
            Height = 48,
            Visibility = Visibility.Collapsed
        };
        stackPanel.Children.Add(button);
        stackPanel.Children.Add(image);
        stackPanel.Children.Add(textBlock);

        return stackPanel;
    }

    public static Image Icon()
    {
        Image image = new()
        {
            Width = 16,
            Height = 16,
            Margin = new(8, 0, 0, 0),
            Source = "Icon.png".AsImage()
        };

        return image;
    }

    public static NavigationView NavigationView()
    {
        Frame frame = new();
        NavigationView navigationView = new()
        {
            Content = frame,
            OpenPaneLength = 200,
            IsSettingsVisible = false
        };

        navigationView.SetBinding(Microsoft.UI.Xaml.Controls.NavigationView.IsBackEnabledProperty, new Binding() { 
            Source = frame, 
            Path = new PropertyPath("CanGoBack"), 
            Mode = BindingMode.TwoWay });

        navigationView.MenuItems.Add(
            new NavigationViewItem() { Content = "Home", Icon = new SymbolIcon(Symbol.Home), Tag = "IcyLauncher.Views.HomeView" 
            });

        return navigationView;
    }
}