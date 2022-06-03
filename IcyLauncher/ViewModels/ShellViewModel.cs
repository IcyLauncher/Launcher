namespace IcyLauncher.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly Configuration configuration;

    public ShellViewModel(ILogger<ShellViewModel> logger, IOptions<Configuration> configuration)
    {
        this.logger = logger;
        this.configuration = configuration.Value;

        this.logger.Log("Window Started");
    }


    public void WindowClosed()
    {
        configuration.Export();

        logger.Log("Window Closed");
    }
}