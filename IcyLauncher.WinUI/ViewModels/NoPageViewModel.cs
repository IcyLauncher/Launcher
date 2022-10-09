namespace IcyLauncher.WinUI.ViewModels;

public partial class NoPageViewModel : ObservableObject
{
    #region Setup
    readonly INavigation navigation;

    public NoPageViewModel(
        INavigation navigation)
    {
        this.navigation = navigation;
    }
    #endregion


    #region Actions
    [RelayCommand]
    void NavigateHome()
    {
        navigation.Navigate("Home");
        navigation.ClearBackStack();
    }

    [RelayCommand]
    void ReportBug() =>
        navigation.Navigate("Help");
    #endregion
}