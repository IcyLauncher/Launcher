using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT;

namespace IcyLauncher.Core.Services;

public class AcrylicBackdropHandler : IBackdropHandler
{
    readonly ILogger logger;
    readonly Window shell;
    readonly WindowsSystemDispatcherQueueHelper dispatcherQueueHelper = new();
    DesktopAcrylicController controller = new();
    SystemBackdropConfiguration backdropConfiguration = new();

    public AcrylicBackdropHandler(ILogger<AcrylicBackdropHandler> logger, Window shell)
    {
        this.logger = logger;
        this.shell = shell;

        this.logger.Log("Registered BackdropHandler");
    }


    public bool SetBackdrop(bool useDarkMode)
    {
        if (!DesktopAcrylicController.IsSupported())
        {
            logger.Log("Failed to set System Backdrop", Exceptions.Unsupported);
            return false;
        }

        try
        {
            dispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();
            logger.Log("Ensured WindowsSystemDispatcherQueueController");

            shell.Activated += ShellActivated;
            shell.Closed += ShellClosed;
            logger.Log("Hooked Activated/Closed handlers");

            backdropConfiguration.IsInputActive = true;
            backdropConfiguration.Theme = useDarkMode ? SystemBackdropTheme.Dark : SystemBackdropTheme.Light;
            logger.Log("Configured Backdrop Configuration");

            controller.AddSystemBackdropTarget(shell.As<ICompositionSupportsSystemBackdrop>());
            controller.SetSystemBackdropConfiguration(backdropConfiguration);
            logger.Log("Set System Backdrop");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to set System Backdrop", ex);
            return false;
        }
    }


    void ShellActivated(object sender, WindowActivatedEventArgs args) =>
        backdropConfiguration.IsInputActive = args.WindowActivationState is not WindowActivationState.Deactivated;

    void ShellClosed(object sender, WindowEventArgs args)
    {
        shell.Activated -= ShellActivated;

        controller.Dispose();
        controller = default!;
        backdropConfiguration = default!;
        logger.Log("Unhooked Backdrophandler");
    }
}