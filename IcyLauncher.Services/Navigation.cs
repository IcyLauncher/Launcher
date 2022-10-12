using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System.Collections.Generic;
using System.Linq;

namespace IcyLauncher.Services;

public class Navigation : INavigation
{
    readonly ILogger logger;
    readonly UIElementReciever uIElementReciever;

    readonly List<NavigationViewItem> items;
    bool skipEvent = false;

    /// <summary>
    /// Service for navigating in the current main window
    /// </summary>
    public Navigation(
        ILogger<Navigation> logger,
        UIElementReciever uIElementReciever)
    {
        this.logger = logger;
        this.uIElementReciever = uIElementReciever;

        items = uIElementReciever.NavigationView.MenuItems.Concat(uIElementReciever.NavigationView.FooterMenuItems).Select(item => (NavigationViewItem)item).ToList();

        uIElementReciever.NavigationView.SelectionChanged += (s, e) =>
        {
            if (!skipEvent && e.SelectedItemContainer is NavigationViewItem item)
                SetCurrentPage($"Views.{item.Tag}View".AsType() is Type type ? type : "Views.NoPageView".AsType());
        };
        uIElementReciever.NavigationView.BackRequested += (s, e) => GoBack();

        logger.Log("Registered navigation");
    }


    /// <summary>
    /// Gets the current navigation view item
    /// </summary>
    /// <returns>The current navigation view item</returns>
    public NavigationViewItem? GetCurrentNavigationViewItem() =>
        uIElementReciever.NavigationView is NavigationView container ? container.SelectedItem is NavigationViewItem current ? current : null : null;

    /// <summary>
    /// Gets the LayoutRoot grid of the current navigation view item
    /// </summary>
    /// <returns>The current navigation view item LayoutRoot</returns>
    public Grid? GetCurrentNavigationViewItemLayoutRoot() =>
        GetCurrentNavigationViewItem() is NavigationViewItem item ? (Grid)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(item, 0), 0), 0) : null;


    /// <summary>
    /// Searches for a specific navigation view item in the current NavigationView
    /// </summary>
    /// <param name="searchFor">The string to search for</param>
    /// <param name="searchForTag">The boolean wether to search in the element tag</param>
    /// <param name="comparision">The string comparison mode which should be applied when searching</param>
    /// <returns>The found navigation view item</returns>
    public NavigationViewItem? GetNavigationViewItem(
        string? searchFor,
        bool searchForTag = false,
        StringComparison comparision = StringComparison.InvariantCultureIgnoreCase)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchFor))
                return null;

            return items.Find(item => searchFor.Equals(searchForTag ? $"IcyLauncher.WinUI.Views.{item.Tag}View" : item.Tag.ToString(), comparision));
        }
        catch { return null; }
    }


    /// <summary>
    /// Sets the current navigation item
    /// </summary>
    /// <param name="item">The navigation view item which should be set</param>
    /// <returns>A boolean wether the navigation view item has been set successfully</returns>
    public bool SetCurrentNavigationViewItem(
        NavigationViewItem item)
    {
        try
        {
            uIElementReciever.NavigationView.SelectedItem = item;

            logger.Log("Set current navigation item");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to set current navigation item", ex);
            return false;
        }
    }

    /// <summary>
    /// Sets the current navigation view item index
    /// </summary>
    /// <param name="index">The index which should be set</param>
    /// <returns>A boolean wether the current navigation view item index has been set successfully</returns>
    public bool SetCurrentIndex(
        int index)
    {
        try
        {
            NavigationViewItem selectedItem = items.ElementAt(index);

            if ((NavigationViewItem)uIElementReciever.NavigationView.SelectedItem == selectedItem)
                return false;

            skipEvent = true;
            uIElementReciever.NavigationView.SelectedItem = selectedItem;
            skipEvent = false;

            logger.Log("Set current navigation item without updating page");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to set current navigation item without updating page", ex);
            return false;
        }
    }

    /// <summary>
    /// Sets the current navigation frame page
    /// </summary>
    /// <param name="type">The page which should be navigated to</param>
    /// <param name="parameter">The parameter which should get passed to</param>
    /// <returns>A boolean wether the page has been set successfully</returns>
    public bool SetCurrentPage(
        Type? type,
        object? parameter = null)
    {
        try
        {
            bool navigate = uIElementReciever.NavigationFrame.Navigate(type, parameter);
            CanGoBackChanged(uIElementReciever.NavigationFrame.CanGoBack);

            logger.Log("Set current navigation page");
            return navigate;
        }
        catch (Exception ex)
        {
            uIElementReciever.NavigationFrame.Navigate("Views.NoPageView".AsType(), ex);
            CanGoBackChanged(uIElementReciever.NavigationFrame.CanGoBack);

            logger.Log("Failed to set current navigation page", ex);
            return false;
        }
    }

    /// <summary>
    /// Navigates to the given navigation view item 
    /// </summary>
    /// <param name="item">The navigation view item which should be set</param>
    /// <returns>A boolean wether the navigation view item has been navigated to successfully</returns>
    public bool Navigate(
        NavigationViewItem? item)
    {
        if (item is null || string.IsNullOrWhiteSpace(item.Tag.ToString()))
            return false;

        if ($"Views.{item.Tag}View".AsType() is not null)
            return SetCurrentNavigationViewItem(item);
        else
            return false;
    }

    /// <summary>
    /// Navigates to the given page 
    /// </summary>
    /// <param name="page">The page item which should be navigated to</param>
    /// <returns>A boolean wether the page has been navigated to successfully</returns>
    public bool Navigate(string page) =>
        Navigate(GetNavigationViewItem(page));


    /// <summary>
    /// Navigates a page back
    /// </summary>
    /// <returns>A boolean wether a page back has been navigated to successfully</returns>
    public bool GoBack()
    {
        try
        {
            if (GetNavigationViewItem(uIElementReciever.NavigationFrame.BackStack.Last().SourcePageType.FullName, true) is NavigationViewItem item && (NavigationViewItem)uIElementReciever.NavigationView.SelectedItem != item)
                SetCurrentNavigationViewItem(item);
            else if (!SetCurrentPage(uIElementReciever.NavigationFrame.BackStack.Last().SourcePageType))
                return false;

            uIElementReciever.NavigationFrame.BackStack.RemoveAt(uIElementReciever.NavigationFrame.BackStackDepth - 1);
            uIElementReciever.NavigationFrame.BackStack.RemoveAt(uIElementReciever.NavigationFrame.BackStackDepth - 1);
            CanGoBackChanged(uIElementReciever.NavigationFrame.CanGoBack);

            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to go back current navigation page", ex);
            return false;
        }
    }

    /// <summary>
    /// Clears the GoBack backstack
    /// </summary>
    public void ClearBackStack()
    {
        uIElementReciever.NavigationFrame.BackStack.Clear();
        CanGoBackChanged(false);
    }

    void CanGoBackChanged(
        bool canGoBack)
    {
        if (canGoBack == (uIElementReciever.BackButton.Opacity != 0))
            return;

        if (canGoBack)
        {
            uIElementReciever.BackButton.Opacity = 1;
            Storyboard board = new();
            board.Children.Add(UIElementProvider.Animate(uIElementReciever.BackButton, "Width", 0, 32, 200));
            board.Begin();
        }
        else
        {
            uIElementReciever.BackButton.Opacity = 0;
            Storyboard board = new();
            board.Children.Add(UIElementProvider.Animate(uIElementReciever.BackButton, "Width", 32, 0, 200));
            board.Begin();
        }
    }
}