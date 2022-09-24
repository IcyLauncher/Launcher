namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    readonly ILogger<ProfilesViewModel> logger;
    readonly ConfigurationManager configurationManager;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;
    readonly IConverter converter;
    readonly IMessage message;
    readonly INavigation navigation;

    public readonly Configuration Configuration;

    public DeveloperSettingsViewModel(
        ILogger<ProfilesViewModel> logger,
        IOptions<Configuration> configuration,
        ConfigurationManager configurationManager,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        IConverter converter,
        IMessage message,
        INavigation navigation)
    {
        this.logger = logger;
        this.configurationManager = configurationManager;
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
        this.converter = converter;
        this.message = message;
        this.navigation = navigation;

        Configuration = configuration.Value;
    }


    public void OnPageLoaded(object _, RoutedEventArgs _1) =>
        navigation.SetCurrentIndex(5);
}