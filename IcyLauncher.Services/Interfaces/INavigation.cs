using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Services.Interfaces;

public interface INavigation
{
    #region NavigationViewItem
    /// <summary>
    /// Gets the current navigation view item
    /// </summary>
    /// <returns>The current navigation view item</returns>
    NavigationViewItem? GetCurrentNavigationViewItem();

    /// <summary>
    /// Gets the LayoutRoot grid of the current navigation view item
    /// </summary>
    /// <returns>The current navigation view item LayoutRoot</returns>
    Grid? GetCurrentNavigationViewItemLayoutRoot();


    /// <summary>
    /// Searches for a specific navigation view item in the current NavigationView
    /// </summary>
    /// <param name="searchFor">The string to search for</param>
    /// <param name="searchForTag">The boolean wether to search in the element tag</param>
    /// <param name="comparision">The string comparison mode which should be applied when searching</param>
    /// <returns>The found navigation view item</returns>
    NavigationViewItem? GetNavigationViewItem(
        string? searchFor,
        bool searchForTag = false,
        StringComparison comparision = StringComparison.InvariantCultureIgnoreCase);
    #endregion


    #region Set
    /// <summary>
    /// Sets the current navigation item
    /// </summary>
    /// <param name="item">The navigation view item which should be set</param>
    /// <returns>A boolean wether the navigation view item has been set successfully</returns>
    bool SetCurrentNavigationViewItem(
        NavigationViewItem item);

    /// <summary>
    /// Sets the current navigation view item index
    /// </summary>
    /// <param name="index">The index which should be set</param>
    /// <returns>A boolean wether the current navigation view item index has been set successfully</returns>
    bool SetCurrentIndex(
        int index);
    #endregion


    #region Navigation
    /// <summary>
    /// Sets the current navigation frame page
    /// </summary>
    /// <param name="type">The page which should be navigated to</param>
    /// <param name="parameter">The parameter which should get passed to</param>
    /// <returns>A boolean wether the page has been set successfully</returns>
    bool SetCurrentPage(
        Type? type,
        object? parameter = null);

    /// <summary>
    /// Navigates to the given page 
    /// </summary>
    /// <param name="page">The page item which should be navigated to</param>
    /// <returns>A boolean wether the page has been navigated to successfully</returns>
    bool Navigate(
        string page);
    #endregion


    #region BackStack
    /// <summary>
    /// Navigates a page back
    /// </summary>
    /// <returns>A boolean wether a page back has been navigated to successfully</returns>
    bool GoBack();

    /// <summary>
    /// Clears the GoBack backstack
    /// </summary>
    void ClearBackStack();
    #endregion
}