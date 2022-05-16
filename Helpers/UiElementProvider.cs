using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace IcyLauncher.Helpers;

public class UiElementProvider
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

    public static Grid TitleBar()
    {
        Grid grid = new()
        {
            Height = 32,
            Background = new SolidColorBrush(Colors.Red),
            Visibility = Visibility.Collapsed
        };

        return grid;
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