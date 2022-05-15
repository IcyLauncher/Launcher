using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Services;

public class Navigation : INavigation
{
    readonly NavigationView navigationView;
    readonly Frame frame;

    readonly ILogger logger = App.Provider.GetRequiredService<ILogger<Navigation>>();

    public Navigation(NavigationView navigationView, Frame frame)
    {
        this.navigationView = navigationView;
        this.frame = frame;

        this.navigationView.SelectionChanged += (s, args) =>
        {
            if (args.SelectedItemContainer is NavigationViewItem item && Type.GetType(item.Tag.ToString()!) is Type type)
                SetCurrentPage(type);
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

            return items.Find(item => searchFor.Equals(searchForTag ? item.Tag.ToString() : item.Content.ToString(), comparision));
        }
        catch { return null; }
    }


    public bool SetCurrentNavigationViewItem(NavigationViewItem item)
    {
        try
        {
            navigationView.SelectedItem = item;

            logger.Log("The current NavigationView item was set successfully");
            return true;
        }
        catch
        {
            logger.Log("The current NavigationView item failed to set");
            return false;
        }
    }

    public bool SetCurrentPage(Type type, object? parameter = null)
    {
        try
        {
            frame.Navigate(type, parameter);

            logger.Log("The current NavigationView page was set successfully");
            return true;
        }
        catch
        {
            logger.Log("The current NavigationView page item failed to set");
            return false;
        }
    }

    public bool Navigate(NavigationViewItem? item)
    {
        if (item is null || string.IsNullOrWhiteSpace(item.Tag.ToString()))
            return false;

        if (Type.GetType(item.Tag.ToString()!) is not null)
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

            logger.Log("The current NavigationView page went back successfully");
            return true;
        }
        catch
        {
            logger.Log("The current NavigationView page item failed to go back");
            return false;
        }
    }
}