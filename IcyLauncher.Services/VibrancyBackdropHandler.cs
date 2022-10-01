using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace IcyLauncher.Services;

public class VibrancyBackdropHandler : IBackdropHandler
{
    readonly ILogger logger;
    readonly ICompositionSupportsSystemBackdrop shell;

    readonly MicaController controller = new();

    /// <summary>
    /// Handler to confgure a vibrancy backdrop effect on the current main window (Win11+)
    /// </summary>
    public VibrancyBackdropHandler(
        ILogger<VibrancyBackdropHandler> logger,
        Window shell)
    {
        this.logger = logger;
        this.shell = (ICompositionSupportsSystemBackdrop)shell;

        controller.SetSystemBackdropConfiguration(new() { Theme = SystemBackdropTheme.Dark });
        controller.Kind = MicaKind.BaseAlt;

        logger.Log("Registered backdrop handler, set configuration and set controller kind");
    }


    /// <summary>
    /// Enables the backdrop effect
    /// </summary>
    /// <returns>A boolean wether the backdrop effect was enabled successfully</returns>
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

    /// <summary>
    /// Disables the backdrop effect
    /// </summary>
    /// <returns>A boolean wether the backdrop effect was disabled successfully</returns>
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
    /// VibrancyBackdropHandler does not implement light/dark mode. This boolean wont affect the backdrop effect
    /// </summary>
    [Obsolete("No implementation for light/dark mode")]
    public bool IsDarkModeEnabled { get; set; } = true;
}