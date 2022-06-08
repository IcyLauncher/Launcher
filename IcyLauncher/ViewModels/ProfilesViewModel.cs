namespace IcyLauncher.ViewModels;

public partial class ProfilesViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly public Configuration Configuration;
    readonly WindowHandler windowHandler;

    public ProfilesViewModel(ILogger<HomeViewModel> logger, IOptions<Configuration> configuration, WindowHandler windowHandler)
    {
        this.logger = logger;
        Configuration = configuration.Value;
        this.windowHandler = windowHandler;
    }
}