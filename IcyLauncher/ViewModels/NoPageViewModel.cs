using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace IcyLauncher.ViewModels;

public partial class NoPageViewModel : ObservableObject
{
    readonly INavigation navigation;

    public NoPageViewModel(INavigation navigation)
    {
        this.navigation = navigation;
    }


    [ICommand]
    void NavigateHome()
    {
        navigation.Navigate("Home");
        navigation.ClearBackStack();
    }

    [ICommand]
    void ReportBug() =>
        navigation.Navigate("Help");
}