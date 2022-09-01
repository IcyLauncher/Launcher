using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT;

namespace IcyLauncher.Services;

public class MicaBackdropHandler : IBackdropHandler
{
    readonly ILogger logger;
    readonly Window shell;

    readonly MicaController controller = new();

    public MicaBackdropHandler(
        ILogger<MicaBackdropHandler> logger,
        Window shell,
        WindowHandler windowHandler)
    {
        this.logger = logger;
        this.shell = shell;

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
            controller.AddSystemBackdropTarget(shell.As<ICompositionSupportsSystemBackdrop>());

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
            controller.RemoveSystemBackdropTarget(shell.As<ICompositionSupportsSystemBackdrop>());

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
            controller.ResetProperties();
            controller.SetSystemBackdropConfiguration(new() { Theme = value ? SystemBackdropTheme.Dark : SystemBackdropTheme.Light });

            isDarkModeEnabled = value;
            logger.Log($"Updated system backdrop dark mode ({value})");
        }
    }
}