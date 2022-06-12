﻿using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Core.Services;

public class Navigation : INavigation
{
    readonly ILogger logger;
    readonly NavigationView navigationView;
    readonly Frame frame;
    readonly Button backButton;

    public Navigation(ILogger<Navigation> logger, NavigationView navigationView, Frame frame, Button backButton)
    {
        this.logger = logger;
        this.navigationView = navigationView;
        this.frame = frame;
        this.backButton = backButton;

        this.navigationView.SelectionChanged += (s, args) =>
        {
            if (args.SelectedItemContainer is NavigationViewItem item && $"Views.{item.Tag}View".AsType() is Type type)
                SetCurrentPage(type);
            else
                this.logger.Log("Failed to set current NavigationView page: Unregistered Type");
        };
        this.navigationView.BackRequested += (s, args) => GoBack();

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

    private void CanGoBackChanged(bool canGoBack)
    {
        if (canGoBack == (backButton.Visibility != Visibility.Collapsed))
            return;

        if (canGoBack)
        {
            backButton.Visibility = Visibility.Visible;
            UIElementProvider.Animate(backButton, "Opacity", 0, 1, 200).Begin();
            UIElementProvider.Animate(backButton, "Width", 0, 32, 110).Begin();
        }
        else
        {
            UIElementProvider.Animate(backButton, "Opacity", 1, 0, 200).Begin();
            var board = UIElementProvider.Animate(backButton, "Width", 32, 0, 110);
            board.Completed += (s, e) => backButton.Visibility = Visibility.Collapsed;
            board.Begin();
        }
    }
}