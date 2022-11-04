namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class INavigationViewModel : ObservableObject
{
    #region Setup
    readonly INavigation navigation;
    readonly IMessage message;

    public INavigationViewModel(
        INavigation navigation,
        IMessage message)
    {
        this.navigation = navigation;
        this.message = message;
    }
    #endregion


    #region GetCurrentNavigationViewItem
    [RelayCommand]
    async Task GetCurrentNavigationViewItemAsync()
    {
        try
        {
            NavigationViewItem? currentNavigationViewItem = navigation.GetCurrentNavigationViewItem();
            string result = currentNavigationViewItem is not null ? currentNavigationViewItem.Tag.ToString()! : "null";

            await message.ShowAsync("navigation.GetCurrentNavigationViewItem()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("navigation.GetCurrentNavigationViewItem()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region GetCurrentNavigationViewItemLayoutRoot
    [RelayCommand]
    async Task GetCurrentNavigationViewItemLayoutRootAsync()
    {
        try
        {
            Grid? currentLayoutRoot = navigation.GetCurrentNavigationViewItemLayoutRoot();
            string result = currentLayoutRoot is not null ? $"Hash Code: {currentLayoutRoot.GetHashCode()}" : "null";

            await message.ShowAsync("navigation.GetCurrentNavigationViewItemLayoutRoot()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("navigation.GetCurrentNavigationViewItemLayoutRoot()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region GetNavigationViewItem
    [ObservableProperty]
    string searchFor = "Home";

    [ObservableProperty]
    bool searchForTag = false;

    [ObservableProperty]
    StringComparison comparision = StringComparison.InvariantCultureIgnoreCase;

    [RelayCommand]
    async Task GetNavigationViewItemAsync()
    {
        try
        {
            NavigationViewItem? currentLayoutRoot = navigation.GetNavigationViewItem(SearchFor, SearchForTag, Comparision);
            string result = currentLayoutRoot is not null ? currentLayoutRoot.Tag.ToString()! : "null";

            await message.ShowAsync("navigation.GetNavigationViewItem()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("navigation.GetNavigationViewItem()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region SetCurrentIndex
    [RelayCommand]
    async Task SetCurrentIndexAsync(
        double index)
    {
        try
        {
            bool result = navigation.SetCurrentIndex(Convert.ToInt32(index));
            await message.ShowAsync("navigation.SetCurrentIndex()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("navigation.SetCurrentIndex()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region SetCurrentPage
    [ObservableProperty]
    string pageType = "Views.HomeView";

    [ObservableProperty]
    string parameter = "Hello World! :)";

    [RelayCommand]
    async Task SetCurrentPageAsync()
    {
        try
        {
            bool result = navigation.SetCurrentPage(PageType.AsType(), Parameter);
            await message.ShowAsync("navigation.SetCurrentPage()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("navigation.SetCurrentPage()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region Navigation
    [RelayCommand]
    async Task NavigateAsync(
        string page)
    {
        try
        {
            bool result = navigation.Navigate(page);
            await message.ShowAsync("navigation.Navigate()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("navigation.Navigate()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region GoBack
    [RelayCommand]
    async Task GoBackAsync()
    {
        try
        {
            bool result = navigation.GoBack();
            await message.ShowAsync("navigation.GoBack()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("navigation.GoBack()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region ClearBackStack
    [RelayCommand]
    async Task ClearBackStackAsync()
    {
        try
        {
            navigation.ClearBackStack();
            await message.ShowAsync("navigation.ClearBackStack()", "Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("navigation.ClearBackStack()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion
}