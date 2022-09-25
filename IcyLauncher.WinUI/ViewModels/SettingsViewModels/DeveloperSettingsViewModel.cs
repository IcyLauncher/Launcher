namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    readonly ILogger<ProfilesViewModel> logger;
    readonly ConfigurationManager configurationManager;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;
    readonly UIElementReciever uiElementReciever;
    readonly IBackdropHandler micaBackdropHandler;
    readonly IBackdropHandler acrylicBackdropHandler;
    readonly IBackdropHandler vibrancyBackdropHandler;
    readonly BackdropHandler backdropHandler;
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
        MicaBackdropHandler micaBackdropHandler,
        AcrylicBackdropHandler acrylicBackdropHandler,
        VibrancyBackdropHandler vibrancyBackdropHandler,
        BackdropHandler backdropHandler,
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
        this.micaBackdropHandler = micaBackdropHandler;
        this.acrylicBackdropHandler = acrylicBackdropHandler;
        this.vibrancyBackdropHandler = vibrancyBackdropHandler;
        this.backdropHandler = backdropHandler;
        this.converter = converter;
        this.fileSystem = fileSystem;
        this.navigation = navigation;
        this.message = message;

        Configuration = configuration.Value;


        SetupWindowHandlerViewModel();
        SetupUIElementRevieverViewModel();
        SetupMicaBackdropHandlerViewModel();
        SetupAcrylicBackdropHandlerViewModel();
        SetupBackdropHandlerViewModel();
    }


    public void OnPageLoaded(object _, RoutedEventArgs _1) =>
        navigation.SetCurrentIndex(5);
}