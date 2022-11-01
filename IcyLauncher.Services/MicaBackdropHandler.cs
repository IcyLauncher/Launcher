using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace IcyLauncher.Services;

public class MicaBackdropHandler : IBackdropHandler
{
    #region Setup
    readonly ILogger logger;
    readonly ICompositionSupportsSystemBackdrop shell;

    readonly MicaController controller = new();

    /// <summary>
    /// Handler to confgure a mica backdrop effect on the current main window (Win11+)
    /// </summary>
    public MicaBackdropHandler(
        ILogger<MicaBackdropHandler> logger,
        CoreWindow shell)
    {
        this.logger = logger;
        this.shell = (ICompositionSupportsSystemBackdrop)(Window)shell;

        IsDarkModeEnabled = true;

        logger.Log("Registered backdrop handler");
    }
    #endregion


    #region Actions
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
    /// Sets and gets the dark mode of the backdrop effect
    /// </summary>
    public bool IsDarkModeEnabled
    {
        get => isDarkModeEnabled;
        set
        {
            controller.SetSystemBackdropConfiguration(new() { Theme = value ? SystemBackdropTheme.Dark : SystemBackdropTheme.Light });

            isDarkModeEnabled = value;
            logger.Log($"Updated system backdrop dark mode ({value})");
        }
    }
    #endregion
}