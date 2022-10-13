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
    #region Setup
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly UIElementReciever uiElementReciever;
    readonly Window shell;

    /// <summary>
    /// Handler to configure the current main window
    /// </summary>
    public WindowHandler(
        ILogger<Window> logger,
        IOptions<Configuration> configuration,
        UIElementReciever uiElementReciever,
        Window shell)
    {
        this.logger = logger;
        this.configuration = configuration.Value;
        this.uiElementReciever = uiElementReciever;
        this.shell = shell;

        logger.Log("Registered window handler");
    }
    #endregion


    #region Information
    AppWindow Window => AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(HWnd));
    object? dispatcherQueueController;

    /// <summary>
    /// HWND of the current main window
    /// </summary>
    public IntPtr HWnd => WindowNative.GetWindowHandle(shell);


    /// <summary>
    /// Boolean wether the current main window has a custom title bar
    /// </summary>
    public bool HasCustomTitleBar { get; private set; }

    /// <summary>
    /// Size of the current main window
    /// </summary>
    public SizeInt32 Size => Window.Size;

    /// <summary>
    /// Position of the current main window
    /// </summary>
    public PointInt32 Position => Window.Position;

    /// <summary>
    /// Size of the current main screen
    /// </summary>
    public RectInt32 ScreenSize => DisplayArea.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(HWnd), DisplayAreaFallback.Nearest).WorkArea;


    /// <summary>
    /// Active logger window (null if none is active)
    /// </summary>
    public Window? LoggerWindow = null;
    #endregion


    #region Icon
    /// <summary>
    /// Sets a custom icon on the current main window
    /// </summary>
    /// <param name="path">The file path to the icon</param>
    public void SetIcon(
        string path)
    {
        Window.SetIcon(path);

        logger.Log($"Set app icon [{path}]");
    }
    #endregion

    #region Size
    /// <summary>
    /// Sets the size of the current main window
    /// </summary>
    /// <param name="width">The width of the new size</param>
    /// <param name="height">The height of the new size</param>
    public void SetSize(
        int width,
        int height)
    {
        Window.Resize(new(width, height));

        logger.Log($"Set window size [{width}x{height}]");
    }
    /// <summary>
    /// Sets the size of the given window
    /// </summary>
    /// <param name="externalWindow">The window to set the size to</param>
    /// <param name="width">The width of the new size</param>
    /// <param name="height">The height of the new size</param>
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

    /// <summary>
    /// Sets the minimum size of the current main window
    /// </summary>
    /// <param name="width">The width of the new size</param>
    /// <param name="height">The height of the new size</param>
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
    #endregion

    #region Position
    /// <summary>
    /// Sets the position of the current main window
    /// </summary>
    /// <param name="x">The x coordinate of the new size</param>
    /// <param name="y">The y coordinate of the new size</param>
    public void SetPosition(
        int x,
        int y)
    {
        Window.Move(new(x, y));

        logger.Log($"Set window position [{x}, {y}]");
    }
    /// <summary>
    /// Sets the position of the current main window to the center of the main screen
    /// </summary>
    public void SetPositionToCenter() =>
        SetPosition((ScreenSize.Width - Window.Size.Width) / 2, (ScreenSize.Height - Window.Size.Height) / 2);
    #endregion


    #region System
    /// <summary>
    /// Initializes a target with the current main window
    /// </summary>
    /// <param name="target">The target to register</param>
    public void Register(object target) =>
        InitializeWithWindow.Initialize(target, HWnd);


    /// <summary>
    /// Ensures there is a windows system dispatcher queue controller
    /// </summary>
    /// <returns>A boolean wether the action was successful</returns>
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
    #endregion


    #region Customization
    /// <summary>
    /// Sets an UIElement as a custom title bar on the current main window
    /// </summary>
    /// <param name="titleBar">The UIElement to set as a title bar</param>
    /// <param name="container">The container UIElement of the title bar to update visibilies</param>
    /// <returns>A boolean wether the UIElement was set as the custom title bar successfully</returns>
    public bool SetTitleBar(
        UIElement? titleBar,
        UIElement? container = null)
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


    /// <summary>
    /// Sets a binded color as the main background of the current main window
    /// </summary>
    /// <param name="backgroundColor">The property path of configuration.Apperance.Colors</param>
    /// <returns>A boolean wether the main background was set successfully</returns>
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
                        Source = configuration.Apperance.Colors,
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
    #endregion
}