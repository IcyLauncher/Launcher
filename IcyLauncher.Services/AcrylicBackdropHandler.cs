using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Windows.UI;

namespace IcyLauncher.Services;

public class AcrylicBackdropHandler : IBackdropHandler
{
    #region Setup
    readonly ILogger logger;
    readonly ICompositionSupportsSystemBackdrop shell;

    readonly DesktopAcrylicController controller = new();

    /// <summary>
    /// Handler to confgure an acrylic backdrop effect on the current main window (Win10+)
    /// </summary>
    public AcrylicBackdropHandler(
        ILogger<AcrylicBackdropHandler> logger,
        CoreWindow shell)
    {
        this.logger = logger;
        this.shell = (ICompositionSupportsSystemBackdrop)(Window)shell;

        controller.SetSystemBackdropConfiguration(new() { Theme = SystemBackdropTheme.Dark });
        controller.LuminosityOpacity = 0.8f;
        controller.TintOpacity = 0.8f;

        IsDarkModeEnabled = true;

        logger.Log("Registered backdrop handler and set configuration");
    }
    #endregion


    #region Actions
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

            controller.TintColor = IsDarkModeEnabled ? Color.FromArgb(255, 50, 50, 50) : Color.FromArgb(230, 255, 255, 255);
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
    #endregion

    #region Theme
    bool isDarkModeEnabled;
    /// <summary>
    /// Gets and sets the dark mode of the backdrop effect
    /// </summary>
    public bool IsDarkModeEnabled
    {
        get => isDarkModeEnabled;
        set
        {
            controller.TintColor = value ? Color.FromArgb(255, 50, 50, 50) : Color.FromArgb(230, 255, 255, 255);

            isDarkModeEnabled = value;
            logger.Log($"Updated system backdrop dark mode ({value})");
        }
    }
    #endregion
}