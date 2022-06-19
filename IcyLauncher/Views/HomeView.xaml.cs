using IcyLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace IcyLauncher.Views;

public sealed partial class HomeView : Page
{
    readonly HomeViewModel viewModel = App.Provider.GetRequiredService<HomeViewModel>();

    public HomeView()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var newScroll = Scrollll.HorizontalOffset - Scrollll.ActualWidth + 50;
        Scrollll.ChangeView(newScroll, null, null);

        bef.Visibility = newScroll <= 0 ? Visibility.Collapsed : Visibility.Visible;
        nex.Visibility = newScroll >= Scrollll.ScrollableWidth ? Visibility.Collapsed : Visibility.Visible;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var newScroll = Scrollll.HorizontalOffset + Scrollll.ActualWidth - 50;
        Scrollll.ChangeView(newScroll, null, null);

        bef.Visibility = newScroll <= 0 ? Visibility.Collapsed : Visibility.Visible;
        nex.Visibility = newScroll >= Scrollll.ScrollableWidth ? Visibility.Collapsed : Visibility.Visible;
    }
}