using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Core.Services;

public class Navigation : INavigation
{
    readonly ILogger logger;
    readonly NavigationView navigationView;
    readonly Frame frame;
    readonly Action<bool> CanGoBackChanged;

    public Navigation(ILogger<Navigation> logger, NavigationView navigationView, Frame frame, Action<bool> CanGoBackChanged)
    {
        this.logger = logger;
        this.navigationView = navigationView;
        this.frame = frame;

        this.CanGoBackChanged = CanGoBackChanged;

        this.navigationView.SelectionChanged += (s, args) =>
        {
            if (args.SelectedItemContainer is NavigationViewItem item && $"IcyLauncher.Views.{item.Tag}View".AsType() is Type type)
                SetCurrentPage(type);
            else
                logger.Log("Failed to set current NavigationView page: Unregistered Type");
        };
        this.navigationView.BackRequested += (s, args) => GoBack();

        logger.Log("Registered NavigationView");
    }


    public NavigationViewItem? GetCurrentNavigationViewItem() =>
        navigationView.SelectedItem is NavigationViewItem current ? current : null;


    public List<NavigationViewItem> GetNavigationViewItems()
    {
        List<NavigationViewItem> result = new();

        var items = navigationView.MenuItems.Select(item => (NavigationViewItem)item).ToList();
        items.AddRange(navigationView.FooterMenuItems.Select(item => (NavigationViewItem)item));

        return items;
    }

    public NavigationViewItem? GetNavigationViewItem(string? searchFor, bool searchForTag = false, StringComparison comparision = StringComparison.InvariantCultureIgnoreCase)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchFor))
                return null;

            var items = GetNavigationViewItems();

            return items.Find(item => searchFor.Equals(searchForTag ? $"IcyLauncher.Views.{item.Tag}View" : item.Tag.ToString(), comparision));
        }
        catch { return null; }
    }


    public bool SetCurrentNavigationViewItem(NavigationViewItem item)
    {
        try
        {
            navigationView.SelectedItem = item;

            logger.Log("Set current NavigationView item");
            return true;
        }
        catch
        {
            logger.Log("Failed to set current NavigationView item");
            return false;
        }
    }

    public bool SetCurrentPage(Type type, object? parameter = null)
    {
        try
        {
            frame.Navigate(type, parameter);
            CanGoBackChanged(frame.CanGoBack);

            logger.Log("Set current NavigationView page");
            return true;
        }
        catch
        {
            logger.Log("Failed to set current NavigationView page");
            return false;
        }
    }

    public bool Navigate(NavigationViewItem? item)
    {
        if (item is null || string.IsNullOrWhiteSpace(item.Tag.ToString()))
            return false;

        if ($"IcyLauncher.Views.{item.Tag}View".AsType() is not null)
            return SetCurrentNavigationViewItem(item);
        else
            return false;
    }
    public bool Navigate(string page)
    {
        var item = GetNavigationViewItem(page);

        return Navigate(item);
    }

    public bool GoBack()
    {
        try
        {
            var item = GetNavigationViewItem(frame.BackStack.Last().SourcePageType.FullName, true);

            if (!Navigate(item))
                return false;

            frame.BackStack.RemoveAt(frame.BackStackDepth - 1);
            frame.BackStack.RemoveAt(frame.BackStackDepth - 1);
            CanGoBackChanged(frame.CanGoBack);

            logger.Log("Current NavigationView page went back");
            return true;
        }
        catch
        {
            logger.Log("Current NavigationView page failed to go back");
            return false;
        }
    }
}