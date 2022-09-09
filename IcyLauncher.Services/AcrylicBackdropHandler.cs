using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace IcyLauncher.Services;

public class AcrylicBackdropHandler : IBackdropHandler
{
    readonly ILogger logger;
    readonly ICompositionSupportsSystemBackdrop shell;
    readonly WindowHandler windowHandler;

    readonly DesktopAcrylicController controller = new();

    public AcrylicBackdropHandler(
        ILogger<AcrylicBackdropHandler> logger,
        Window shell,
        WindowHandler windowHandler)
    {
        this.logger = logger;
        this.shell = (ICompositionSupportsSystemBackdrop)shell;
        this.windowHandler = windowHandler;

        controller.SetSystemBackdropConfiguration(new() { Theme = SystemBackdropTheme.Dark });

        IsDarkModeEnabled = true;

        logger.Log("Registered backdrop handler and set backdrop configuration");
    }


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