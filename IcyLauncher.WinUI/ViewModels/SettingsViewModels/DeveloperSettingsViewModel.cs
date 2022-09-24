namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    readonly ILogger<ProfilesViewModel> logger;
    readonly ConfigurationManager configurationManager;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;
    readonly UIElementReciever uiElementReciever;
    readonly IConverter converter;
    readonly IFileSystem fileSystem;
    readonly INavigation navigation;
    readonly IMessage message;

    public readonly Configuration Configuration;

    public DeveloperSettingsViewModel(
        ILogger<ProfilesViewModel> logger,
        IOptions<Configuration> configuration,
        ConfigurationManager configurationManager,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        UIElementReciever uiElementReciever,
        IConverter converter,
        IFileSystem fileSystem,
        INavigation navigation,
        IMessage message)
    {
        this.logger = logger;
        this.configurationManager = configurationManager;
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
        this.uiElementReciever = uiElementReciever;
        this.converter = converter;
        this.fileSystem = fileSystem;
        this.navigation = navigation;
        this.message = message;

        Configuration = configuration.Value;


        SetupWindowHandlerViewModel();
    }


    public void OnPageLoaded(object _, RoutedEventArgs _1) =>
        navigation.SetCurrentIndex(5);
}