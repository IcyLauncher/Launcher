namespace IcyLauncher.WinUI.ViewModels;

public partial class NoPageViewModel : ObservableObject
{
    readonly INavigation navigation;

    public NoPageViewModel(
        INavigation navigation)
    {
        this.navigation = navigation;
    }


    [RelayCommand]
    void NavigateHome()
    {
        navigation.Navigate("Home");
        navigation.ClearBackStack();
    }

    [RelayCommand]
    void ReportBug() =>
        navigation.Navigate("Help");
}