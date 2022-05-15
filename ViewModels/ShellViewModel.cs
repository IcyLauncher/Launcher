namespace IcyLauncher.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly INavigation navigation;

    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<Configuration> configuration, INavigation navigation)
    {
        this.logger = logger;
        this.configuration = configuration.Value;
        this.navigation = navigation;
    }

    public void WindowActivated() =>
        navigation.Navigate("Home");

    public void WindowClosed() =>
        configuration.Export();
}