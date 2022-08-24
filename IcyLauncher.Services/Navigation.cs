using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Collections.Generic;
using System.Linq;

namespace IcyLauncher.Services;

public class Navigation : INavigation
{
    readonly ILogger logger;
    readonly NavigationView navigationView;
    readonly Frame frame;
    readonly Button backButton;
    readonly IEnumerable<object> navigationViewItems;

    bool skipEvent = false;

    public Navigation(ILogger<Navigation> logger, NavigationView navigationView, Frame frame, Button backButton)
    {
        this.logger = logger;
        this.navigationView = navigationView;
        this.frame = frame;
        this.backButton = backButton;

        navigationViewItems = this.navigationView.MenuItems.Concat(navigationView.FooterMenuItems);

        this.navigationView.SelectionChanged += (s, e) =>
        {
            if (!skipEvent && e.SelectedItemContainer is NavigationViewItem item)
                SetCurrentPage($"Views.{item.Tag}View".AsType() is Type type ? type : "Views.NoPageView".AsType());
        };
        this.navigationView.BackRequested += (s, e) => GoBack();

        this.logger.Log("Registered NavigationView");
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

            return items.Find(item => searchFor.Equals(searchForTag ? $"IcyLauncher.WinUI.Views.{item.Tag}View" : item.Tag.ToString(), comparision));
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
        catch (Exception ex)
        {
            logger.Log("Failed to set current NavigationView item", ex);
            return false;
        }
    }
    public bool SetCurrentIndex(int index)
    {
        try
        {
            skipEvent = true;
            navigationView.SelectedItem = navigationViewItems.ElementAt(index);
            skipEvent = false;

            logger.Log("Set current NavigationView item without updating page");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to set current NavigationView item", ex);
            return false;
        }
    }

    public bool SetCurrentPage(Type? type, object? parameter = null)
    {
        try
        {
            var navigate = frame.Navigate(type, parameter);
            CanGoBackChanged(frame.CanGoBack);

            logger.Log("Set current NavigationView page");
            return navigate;
        }
        catch (Exception ex)
        {
            frame.Navigate("Views.NoPageView".AsType(), ex);
            CanGoBackChanged(frame.CanGoBack);

            logger.Log("Failed to set current NavigationView page", ex);
            return false;
        }
    }

    public bool Navigate(NavigationViewItem? item)
    {
        if (item is null || string.IsNullOrWhiteSpace(item.Tag.ToString()))
            return false;

        if ($"Views.{item.Tag}View".AsType() is not null)
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
            logger.Log(frame.BackStack.Last().SourcePageType.FullName);
            if (GetNavigationViewItem(frame.BackStack.Last().SourcePageType.FullName, true) is NavigationViewItem item && (NavigationViewItem)navigationView.SelectedItem != item)
                SetCurrentNavigationViewItem(item);
            else if (!SetCurrentPage(frame.BackStack.Last().SourcePageType))
                return false;

            frame.BackStack.RemoveAt(frame.BackStackDepth - 1);
            frame.BackStack.RemoveAt(frame.BackStackDepth - 1);
            CanGoBackChanged(frame.CanGoBack);

            logger.Log("Current NavigationView page went back");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Current NavigationView page failed to go back", ex);
            return false;
        }
    }

    public void ClearBackStack()
    {
        frame.BackStack.Clear();
        CanGoBackChanged(false);
    }

    private void CanGoBackChanged(bool canGoBack)
    {
        if (canGoBack == (backButton.Opacity != 0))
            return;

        if (canGoBack)
        {
            backButton.Opacity = 1;
            var board = new Storyboard();
            board.Children.Add(UIElementProvider.Animate(backButton, "Width", 0, 32, 200));
            board.Begin();
        }
        else
        {
            backButton.Opacity = 0;
            var board = new Storyboard();
            board.Children.Add(UIElementProvider.Animate(backButton, "Width", 32, 0, 200));
            board.Begin();
        }
    }
}