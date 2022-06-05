using Microsoft.Extensions.Options;
using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Windows.UI;
using WinRT;

namespace IcyLauncher.Core.Services;

public class MicaBackdropHandler : IBackdropHandler
{
    readonly ILogger logger;
    readonly Configuration configuration;

    public Window Shell { get; private set; }
    public object Controller { get; private set; } = new MicaController();
    public SystemBackdropConfiguration BackdropConfiguration { get; private set; } = new();
    public WindowsSystemDispatcherQueueHelper DispatcherQueueHelper { get; set; } = new();

    public MicaBackdropHandler(ILogger<MicaBackdropHandler> logger, IOptions<Configuration> configuration, Window shell)
    {
        this.logger = logger;
        this.configuration = configuration.Value;

        Shell = shell;

        this.logger.Log("Registered BackdropHandler: Mica");
    }


    public bool SetBackdrop()
    {
        if (!MicaController.IsSupported())
            return false;

        DispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();
        logger.Log("Ensured WindowsSystemDispatcherQueueController");

        Shell.Activated += ShellActivated;
        Shell.Closed += ShellClosed;
        logger.Log("Hooked Activated/Closed handlers");

        BackdropConfiguration.IsInputActive = true;
        BackdropConfiguration.Theme = configuration.Apperance.Colors.Background.Primary != Color.FromArgb(255, 32, 32, 32) ? SystemBackdropTheme.Light : SystemBackdropTheme.Dark;
        logger.Log("Configured Backdrop Configuration");

        ((MicaController)Controller).AddSystemBackdropTarget(Shell.As<ICompositionSupportsSystemBackdrop>());
        ((MicaController)Controller).SetSystemBackdropConfiguration(BackdropConfiguration);
        logger.Log("Set System Backdrop");
        return true;
    }


    void ShellActivated(object sender, WindowActivatedEventArgs args) =>
        BackdropConfiguration.IsInputActive = args.WindowActivationState is not WindowActivationState.Deactivated;

    void ShellClosed(object sender, WindowEventArgs args)
    {
        ((MicaController)Controller).Dispose();
        Controller = null!;
        Shell.Activated -= ShellActivated;
        BackdropConfiguration = null!;
        logger.Log("Unhooked Backdrophandler: Mica");
    }
}