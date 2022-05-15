using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.Services.Interfaces;

interface INavigation
{
    static NavigationView CreateNew()
    {
        Frame frame = new();
        NavigationView navigationView = new()
        {
            Content = frame,
            OpenPaneLength = 200,
            IsSettingsVisible = false
        };

        navigationView.SetBinding(NavigationView.IsBackEnabledProperty, new Binding() { Source = frame, Path = new PropertyPath("CanGoBack"), Mode = BindingMode.TwoWay });

        navigationView.MenuItems.Add(new NavigationViewItem() { Content = "Home", Icon = new SymbolIcon(Symbol.Home), Tag = "MVVMTest.Views.HomeView" });
        navigationView.MenuItems.Add(new NavigationViewItem() { Content = "Second", Icon = new SymbolIcon(Symbol.TwoBars), Tag = "MVVMTest.Views.SecondView" });

        return navigationView;
    }


    NavigationViewItem? GetCurrentNavigationViewItem();


    List<NavigationViewItem> GetNavigationViewItems();

    NavigationViewItem? GetNavigationViewItem(string? searchFor, bool searchForTag, StringComparison comparision);


    bool SetCurrentNavigationViewItem(NavigationViewItem item);

    bool SetCurrentPage(Type type, object? parameter = null);

    bool Navigate(string page);

    bool GoBack();
}