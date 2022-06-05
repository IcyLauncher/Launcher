using Microsoft.UI;

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

    [ObservableProperty]
    bool enable = true;
    [ObservableProperty]
    bool darkMOde = true;

    [ICommand]
    void Mica()
    {
        windowHandler.SetBlur(BlurEffect.Mica, Enable, DarkMOde);
        logger.Log("\n\n\n");
    }
    [ICommand]
    void Acrylic()
    {
        windowHandler.SetBlur(BlurEffect.Acrylic, Enable, DarkMOde);
        logger.Log("\n\n\n");
    }
    [ICommand]
    void Simple()
    {
        windowHandler.SetBlur(BlurEffect.Simple, Enable, DarkMOde);
        logger.Log("\n\n\n");
    }
    [ICommand]
    void None()
    {
        windowHandler.SetBlur(BlurEffect.None, Enable, DarkMOde);
        logger.Log("\n\n\n");
    }

    [ICommand]
    void ReColor()
    {
        Configuration.Apperance.Colors.Accent.Primary = Colors.Red;
    }
}