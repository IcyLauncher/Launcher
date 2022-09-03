using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT;

namespace IcyLauncher.Services;

public class VibrancyBackdropHandler : IBackdropHandler
{
    readonly ILogger logger;
    readonly ICompositionSupportsSystemBackdrop shell;

    readonly MicaController controller = new();

    public VibrancyBackdropHandler(
        ILogger<VibrancyBackdropHandler> logger,
        Window shell,
        WindowHandler windowHandler)
    {
        this.logger = logger;
        this.shell = (ICompositionSupportsSystemBackdrop)shell;

        windowHandler.EnsureWindowsSystemDispatcherQueueController();
        controller.SetSystemBackdropConfiguration(new() { Theme = SystemBackdropTheme.Dark });
        controller.Kind = MicaKind.BaseAlt;

        IsDarkModeEnabled = true;

        logger.Log("Registered backdrop handler and set backdrop configuration");
    }


    public bool EnableBackdrop()
    {
        if (!MicaController.IsSupported())
        {
            logger.Log("Failed to set system backdrop", Exceptions.Unsupported);
            return false;
        }

        try
        {
            if (shell.SystemBackdrop is not null)
            {
                logger.Log("Failed to set system backdrop", Exceptions.IsNotNull);
                return false;
            }

            controller.AddSystemBackdropTarget(shell);

            logger.Log("Set system backdrop");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to set system backdrop", ex);
            return false;
        }
    }

    public bool DisableBackdrop()
    {
        try
        {
            controller.ResetProperties();
            controller.Kind = MicaKind.BaseAlt;
            controller.RemoveSystemBackdropTarget(shell);

            logger.Log("Disabled system backdrop and reset controller");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to disable system backdrop and reset controller", ex);
            return false;
        }
    }

    /// <summary>
    /// VibrancyBackdropHandler does not implement light/dark mode. This boolean wont affect this system backdrop.
    /// </summary>
    public bool IsDarkModeEnabled { get; set; } = true;
}