namespace IcyLauncher.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly ConfigurationManager configurationManagaer;

    public ShellViewModel(ILogger<ShellViewModel> logger, ConfigurationManager configurationManagaer)
    {
        this.logger = logger;
        this.configurationManagaer = configurationManagaer;

        this.logger.Log("Window Started");
    }


    public void WindowClosed()
    {
        configurationManagaer.Export();

        logger.Log("Window Closed");
    }
}