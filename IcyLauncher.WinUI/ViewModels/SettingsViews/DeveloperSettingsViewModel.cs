namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    readonly INavigation navigation;

    public readonly Configuration Configuration;

    public DeveloperSettingsViewModel(IOptions<Configuration> configuration,
        INavigation navigation)
    {
        this.navigation = navigation;

        Configuration = configuration.Value;
    }


    public void OnPageLoaded(object _, RoutedEventArgs _1) =>
        navigation.SetCurrentIndex(5);
}