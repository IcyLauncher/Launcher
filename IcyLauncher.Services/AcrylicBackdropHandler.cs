using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace IcyLauncher.Services;

public class AcrylicBackdropHandler : IBackdropHandler
{
    readonly ILogger logger;
    readonly WindowHandler windowHandler;
    readonly ICompositionSupportsSystemBackdrop shell;

    readonly DesktopAcrylicController controller = new();

    /// <summary>
    /// Handler to confgure an acrylic backdrop effect on the current main window (Win10+)
    /// </summary>
    public AcrylicBackdropHandler(
        ILogger<AcrylicBackdropHandler> logger,
        WindowHandler windowHandler,
        Window shell)
    {
        this.logger = logger;
        this.windowHandler = windowHandler;
        this.shell = (ICompositionSupportsSystemBackdrop)shell;

        controller.SetSystemBackdropConfiguration(new() { Theme = SystemBackdropTheme.Dark });

        IsDarkModeEnabled = true;

        logger.Log("Registered backdrop handler and set configuration");
    }

    /// <summary>
    /// Enables the backdrop
    /// </summary>
    /// <returns>A boolean wether the backdrop effect was enabled successfully</returns>
    public bool EnableBackdrop()
    {
        if (!DesktopAcrylicController.IsSupported())
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

            controller.LuminosityOpacity = IsDarkModeEnabled ? 0 : 1;
            windowHandler.SetMainBackground("Background.Transparent");
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
    /// Disables the backdrop
    /// </summary>
    /// <returns>A boolean wether the backdrop effect was disabled successfully</returns>
    public bool DisableBackdrop()
    {
        try
        {
            controller.ResetProperties();
            windowHandler.SetMainBackground("Transparent");
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


    bool isDarkModeEnabled;
    /// <summary>
    /// Gets and sets the dark mode of the backdrop effect
    /// </summary>
    public bool IsDarkModeEnabled
    {
        get => isDarkModeEnabled;
        set
        {
            controller.LuminosityOpacity = value ? 0 : 1;

            isDarkModeEnabled = value;
            logger.Log($"Updated system backdrop dark mode ({value})");
        }
    }
}