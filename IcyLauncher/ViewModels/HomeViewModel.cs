namespace IcyLauncher.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly public Configuration Configuration;
    readonly WindowHandler windowHandler;
    IBackdropHandler handy;

    public HomeViewModel(ILogger<ShellViewModel> logger, IOptions<Configuration> configuration, WindowHandler windowHandler, IBackdropHandler handy)
    {
        this.logger = logger;
        Configuration = configuration.Value;
        this.windowHandler = windowHandler;
        this.handy = handy;
    }

    [ICommand]
    void Ass()
    {
        handy.SetBackdrop();
    }
}