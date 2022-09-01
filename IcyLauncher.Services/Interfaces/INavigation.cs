using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Services.Interfaces;

public interface INavigation
{
    NavigationViewItem? GetCurrentNavigationViewItem();

    Grid? GetCurrentNavigationViewItemLayoutRoot();


    NavigationViewItem? GetNavigationViewItem(string? searchFor, bool searchForTag = false, StringComparison comparision = StringComparison.InvariantCultureIgnoreCase);


    bool SetCurrentNavigationViewItem(NavigationViewItem item);

    bool SetCurrentIndex(int index);

    bool SetCurrentPage(Type? type, object? parameter = null);

    bool Navigate(string page);

    bool GoBack();

    void ClearBackStack();
}