using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT;

namespace IcyLauncher.Services;

public class MicaBackdropHandler : IBackdropHandler
{
    readonly ILogger logger;
    readonly ICompositionSupportsSystemBackdrop shell;

    readonly MicaController controller = new();

    public MicaBackdropHandler(
        ILogger<MicaBackdropHandler> logger,
        Window shell,
        WindowHandler windowHandler)
    {
        this.logger = logger;
        this.shell = (ICompositionSupportsSystemBackdrop)shell;

        windowHandler.EnsureWindowsSystemDispatcherQueueController();

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
            controller.SetSystemBackdropConfiguration(new() { Theme = value ? SystemBackdropTheme.Dark : SystemBackdropTheme.Light });

            isDarkModeEnabled = value;
            logger.Log($"Updated system backdrop dark mode ({value})");
        }
    }
}