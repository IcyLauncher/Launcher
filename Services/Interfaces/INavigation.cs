using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.Services.Interfaces;

public interface INavigation
{
    NavigationViewItem? GetCurrentNavigationViewItem();


    List<NavigationViewItem> GetNavigationViewItems();

    NavigationViewItem? GetNavigationViewItem(string? searchFor, bool searchForTag, StringComparison comparision);


    bool SetCurrentNavigationViewItem(NavigationViewItem item);

    bool SetCurrentPage(Type type, object? parameter = null);

    bool Navigate(string page);

    bool GoBack();
}