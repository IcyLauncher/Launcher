using IcyLauncher.Xaml.Converters;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System.Runtime.InteropServices;
using Windows.Graphics;
using Windows.System;
using WinRT.Interop;

namespace IcyLauncher.Services;

public class WindowHandler
{
    readonly ILogger logger;
    readonly ThemeManager themeManager;
    readonly UIElementReciever uiElementReciever;
    readonly Window shell;

    public WindowHandler(
        ILogger<Window> logger,
        ThemeManager themeManager,
        UIElementReciever uiElementReciever,
        Window shell)
    {
        this.logger = logger;
        this.themeManager = themeManager;
        this.uiElementReciever = uiElementReciever;
        this.shell = shell;

        logger.Log("Registered window handler");
    }


    AppWindow Window => AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(HWnd));
    object? dispatcherQueueController;

    public IntPtr HWnd => WindowNative.GetWindowHandle(shell);

    public bool HasCustomTitleBar { get; private set; }
    public SizeInt32 Size => Window.Size;
    public PointInt32 Position => Window.Position;
    public RectInt32 ScreenSize => DisplayArea.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(HWnd), DisplayAreaFallback.Nearest).WorkArea;

    public Window? LoggerWindow = null;


    public void SetIcon(
        string path)
    {
        Window.SetIcon(path);

        logger.Log($"Set app icon [{path}]");
    }

    public void SetSize(
        int width,
        int height)
    {
        Window.Resize(new(width, height));

        logger.Log($"Set window size [{width}x{height}]");
    }
    public void SetSize(
        Window externalWindow,
        int width,
        int height)
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(externalWindow);
        AppWindow window = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(hWnd));

        window.Resize(new(width, height));

        logger.Log($"Set external window size [{width}x{height}]");
    }

    public void SetMinSize(
        int width,
        int height)
    {
        IntPtr dpi = Win32.GetDpiForWindow(HWnd);

        Win32.MinWidth = (int)(width * (float)dpi / 96);
        Win32.MinHeight = (int)(height * (float)dpi / 96);

        Win32.NewWndProc = new Win32.WinProc(Win32.NewWindowProc);
        Win32.OldWndProc = Win32.SetWindowLong(HWnd, -16 | 0x4 | 0x8, Win32.NewWndProc);

        logger.Log($"Set window minimum size [{width}x{height}]");
    }

    public void SetPosition(
        int x,
        int y)
    {
        Window.Move(new(x, y));

        logger.Log($"Set window position [{x}, {y}]");
    }
    public void SetPositionToCenter() =>
        SetPosition((ScreenSize.Width - Window.Size.Width) / 2, (ScreenSize.Height - Window.Size.Height) / 2);


    public void Register(object target) =>
        InitializeWithWindow.Initialize(target, HWnd);


    public bool EnsureWindowsSystemDispatcherQueueController()
    {
        if (DispatcherQueue.GetForCurrentThread() is not null || dispatcherQueueController is not null)
        {
            logger.Log("Failed to ensure DispatcherQueueController", Exceptions.IsNotNull);
            return false;
        }

        Win32.CreateDispatcherQueueController(new()
        {
            dwSize = Marshal.SizeOf(typeof(Win32.DISPATCHERQUEUEOPTIONS)),
            threadType = 2,
            apartmentType = 2
        }, ref dispatcherQueueController);

        logger.Log("Ensured DispatcherQueueController");
        return true;
    }


    public bool SetTitleBar(UIElement? titleBar, UIElement? container = null)
    {
        try
        {
            if (titleBar is null)
            {
                if (container is not null)
                    container.Visibility = Visibility.Collapsed;

                shell.ExtendsContentIntoTitleBar = false;
                shell.SetTitleBar(null);

                HasCustomTitleBar = false;

                logger.Log("Removed custom TitleBar");
                return true;
            }

            if (container is not null)
                container.Visibility = Visibility.Visible;

            shell.ExtendsContentIntoTitleBar = true;
            shell.SetTitleBar(titleBar);

            HasCustomTitleBar = true;

            logger.Log("Set custom TitleBar");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to set custom TitleBar", ex);
            return false;
        }
    }


    public bool SetMainBackground(
        string backgroundColor)
    {
        try
        {
            switch (backgroundColor)
            {
                case "Transparent":
                    uiElementReciever.MainGrid.Background = Colors.Transparent.AsSolid();
                    break;
                default:
                    uiElementReciever.MainGrid.SetBinding(Panel.BackgroundProperty, new Binding()
                    {
                        Source = themeManager.Colors,
                        Converter = new ColorBrushConverter(),
                        Path = new PropertyPath(backgroundColor),
                        Mode = BindingMode.OneWay
                    });
                    break;
            }

            logger.Log($"Set background color on MainGrid [{backgroundColor}]");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log($"Failed to set background color on MainGrid [{backgroundColor}]", ex);
            return false;
        }
    }
}