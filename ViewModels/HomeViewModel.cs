namespace IcyLauncher.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly public Configuration Configuration;
    readonly WindowHandler windowHandler;

    public HomeViewModel(ILogger<ShellViewModel> logger, IOptions<Configuration> configuration, WindowHandler windowHandler)
    {
        this.logger = logger;
        Configuration = configuration.Value;
        this.windowHandler = windowHandler;
    }

    [ICommand]
    void Ass()
    {
        windowHandler.SetPositionToCenter();
        logger.Log("your mom");
    }
}