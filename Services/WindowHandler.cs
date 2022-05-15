using IcyLauncher.Views;
using WinRT.Interop;

namespace IcyLauncher.Services;

public class WindowHandler
{
    readonly ILogger logger;
    readonly ShellView shellView;

    public WindowHandler(ILogger<Window> logger)
    {
        this.logger = logger;
        shellView = App.Provider.GetRequiredService<ShellView>();

        this.logger.Log("Registered WindowHandler");
    }

    public IntPtr Hwnd => WindowNative.GetWindowHandle(shellView);
}