using IcyLauncher.Helpers;
using IcyLauncher.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using System.Runtime.InteropServices;
using Windows.Graphics;
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


    AppWindow Window => AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(HWnd));
    OverlappedPresenter Presenter => (OverlappedPresenter)Window.Presenter;

    public IntPtr HWnd => WindowNative.GetWindowHandle(shellView);

    public SizeInt32 Size => Window.Size;
    public PointInt32 Position => Window.Position;
    public SizeInt32 ScreenSize
    {
        get
        {
            IntPtr monitor = Win32.MonitorFromWindow(HWnd, 0);

            var info = new Win32.MONITORINFOEX();
            Win32.GetMonitorInfo(monitor, info);

            logger.Log("Requested screen size");
            return new(info.rcMonitor.right, info.rcMonitor.bottom);
        }
    }


    public void SetIcon(string path)
    {
        Window.SetIcon(path);

        logger.Log($"Set app icon to \"{path}\"");
    }

    public void SetSize(int width, int height)
    {
        Window.Resize(new(width, height));

        logger.Log($"Set window size to \"{width}x{height}\"");
    }

    public void SetMinSize(int width, int height)
    {
        var dpi = Win32.GetDpiForWindow(HWnd);

        Win32.MinWidth = (int)(width * (float)dpi / 96);
        Win32.MinHeight = (int)(height * (float)dpi / 96);

        Win32.NewWndProc = new Win32.WinProc(Win32.NewWindowProc);
        Win32.OldWndProc = Win32.SetWindowLong(HWnd, -16 | 0x4 | 0x8, Win32.NewWndProc);

        logger.Log($"Set window minimum size to \"{width}x{height}\"");
    }

    public void SetPosition(int x, int y)
    {
        Window.Move(new(x, y));

        logger.Log($"Set window position to \"{x}, {y}\"");
    }

    public void SetToCenter()
    {
        var screen = ScreenSize;
        SetPosition(screen.Width / 2 - Size.Width / 2, screen.Height / 2 - Size.Height / 2);
    }
}