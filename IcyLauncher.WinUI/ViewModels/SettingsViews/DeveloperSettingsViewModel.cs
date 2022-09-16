namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    readonly INavigation navigation;
    readonly ILogger<ProfilesViewModel> logger;

    public readonly Configuration Configuration;

    public DeveloperSettingsViewModel(
        ILogger<ProfilesViewModel> logger,
        IOptions<Configuration> configuration,
        INavigation navigation)
    {
        this.logger = logger;
        this.navigation = navigation;

        Configuration = configuration.Value;
    }


    public void OnPageLoaded(object _, RoutedEventArgs _1) =>
        navigation.SetCurrentIndex(5);
}