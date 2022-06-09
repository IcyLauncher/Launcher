using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT;

namespace IcyLauncher.Core.Services;

public class AcrylicBackdropHandler : IBackdropHandler
{
    readonly ILogger logger;

    public Window Shell { get; private set; }
    public object Controller { get; private set; } = new DesktopAcrylicController();
    public SystemBackdropConfiguration BackdropConfiguration { get; private set; } = new();
    public WindowsSystemDispatcherQueueHelper DispatcherQueueHelper { get; set; } = new();

    public AcrylicBackdropHandler(ILogger<AcrylicBackdropHandler> logger, Window shell)
    {
        this.logger = logger;

        Shell = shell;

        this.logger.Log("Registered BackdropHandler: Acrylic");
    }


    public bool SetBackdrop(bool useDarkMode)
    {
        if (!DesktopAcrylicController.IsSupported())
            return false;

        DispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();
        logger.Log("Ensured WindowsSystemDispatcherQueueController");

        Shell.Activated += ShellActivated;
        Shell.Closed += ShellClosed;
        logger.Log("Hooked Activated/Closed handlers");

        BackdropConfiguration.IsInputActive = true;
        BackdropConfiguration.Theme = useDarkMode ? SystemBackdropTheme.Dark : SystemBackdropTheme.Light;
        logger.Log("Configured Backdrop Configuration");

        ((DesktopAcrylicController)Controller).AddSystemBackdropTarget(Shell.As<ICompositionSupportsSystemBackdrop>());
        ((DesktopAcrylicController)Controller).SetSystemBackdropConfiguration(BackdropConfiguration);
        logger.Log("Set System Backdrop");
        return true;
    }


    void ShellActivated(object sender, WindowActivatedEventArgs args) =>
        BackdropConfiguration.IsInputActive = args.WindowActivationState is not WindowActivationState.Deactivated;

    void ShellClosed(object sender, WindowEventArgs args)
    {
        ((DesktopAcrylicController)Controller).Dispose();
        Controller = null!;
        Shell.Activated -= ShellActivated;
        BackdropConfiguration = null!;
        logger.Log("Unhooked Backdrophandler: Acrylic");
    }
}