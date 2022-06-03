using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Windows.UI;
using WinRT.Interop;

namespace IcyLauncher.Core.Services;

public class WindowHandler
{
    readonly ILogger logger;
    readonly Window shell;

    public WindowHandler(ILogger<Window> logger, Window shell)
    {
        this.logger = logger;
        this.shell = shell;

        this.logger.Log("Registered WindowHandler");
    }


    AppWindow Window => AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(HWnd));
    OverlappedPresenter Presenter => (OverlappedPresenter)Window.Presenter;

    public IntPtr HWnd => WindowNative.GetWindowHandle(shell);

    public bool HasTilteBar { get; private set; } = true;
    public SizeInt32 Size => Window.Size;
    public PointInt32 Position => Window.Position;
    public RectInt32 ScreenSize => DisplayArea.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(HWnd), DisplayAreaFallback.Nearest).WorkArea;


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
    public void SetPositionToCenter() =>
        SetPosition((ScreenSize.Width - Window.Size.Width) / 2, (ScreenSize.Height - Window.Size.Height) / 2);

    public bool SetTilteBar(bool isEnabled = false, UIElement? titleBar = null)
    {
        if (!AppWindowTitleBar.IsCustomizationSupported()) 
        {
            logger.Log("Tried to set TitleBar: Not supported");
            return false;
        }

        if (!isEnabled)
        {
            if (titleBar is not null)
                titleBar.Visibility = Visibility.Collapsed;
            HasTilteBar = false;

            Presenter.SetBorderAndTitleBar(true, false);

            logger.Log("Set TitleBar to nothing");
            return true;
        }

        if (titleBar is null)
        {
            HasTilteBar = false;

            Presenter.SetBorderAndTitleBar(true, true);

            Window.TitleBar.ExtendsContentIntoTitleBar = false;

            logger.Log("Set TitleBar to default");
            return true;
        }


        titleBar.Visibility = Visibility.Visible;
        HasTilteBar = true;

        Presenter.SetBorderAndTitleBar(true, true);

        Window.TitleBar.ExtendsContentIntoTitleBar = true;
        Window.TitleBar.SetDragRectangles(new RectInt32[] { new(40, 0, ScreenSize.Width, 48) } );

        Window.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        Window.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(90, 255, 255, 255);
        Window.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(50, 255, 255, 255);
        Window.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        Window.TitleBar.ButtonInactiveForegroundColor = Colors.LightGray;

        shell.SetTitleBar(titleBar);

        logger.Log("Set TitleBar to UIElement");
        return true;
    }
}