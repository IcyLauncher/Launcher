namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    readonly INavigation navigation;
    readonly ILogger<ProfilesViewModel> logger;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;

    public readonly Configuration Configuration;

    public DeveloperSettingsViewModel(
        ILogger<ProfilesViewModel> logger,
        IOptions<Configuration> configuration,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        INavigation navigation)
    {
        this.logger = logger;
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
        this.navigation = navigation;

        Configuration = configuration.Value;
    }


    public void OnPageLoaded(object _, RoutedEventArgs _1) =>
        navigation.SetCurrentIndex(5);
}