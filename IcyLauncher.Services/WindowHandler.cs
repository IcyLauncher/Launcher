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
    OverlappedPresenter Presenter => (OverlappedPresenter)Window.Presenter;
    object? dispatcherQueueController;

    public IntPtr HWnd => WindowNative.GetWindowHandle(shell);

    public bool HasTilteBar { get; private set; } = true;
    public SizeInt32 Size => Window.Size;
    public PointInt32 Position => Window.Position;
    public RectInt32 ScreenSize => DisplayArea.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(HWnd), DisplayAreaFallback.Nearest).WorkArea;


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


    public bool SetTitleBar(
        bool isEnabled = false,
        UIElement? titleBar = null)
    {
        if (!AppWindowTitleBar.IsCustomizationSupported())
        {
            logger.Log("Failed to set TitleBar", Exceptions.Unsupported);
            return false;
        }

        if (!isEnabled)
        {
            if (titleBar is not null)
                titleBar.Visibility = Visibility.Collapsed;
            HasTilteBar = false;

            Presenter.SetBorderAndTitleBar(true, false);

            logger.Log("Set TitleBar [null]");
            return true;
        }

        if (titleBar is null)
        {
            HasTilteBar = false;

            Presenter.SetBorderAndTitleBar(true, true);

            Window.TitleBar.ExtendsContentIntoTitleBar = false;

            logger.Log("Set TitleBar [default]");
            return true;
        }

        try
        {
            titleBar.Visibility = Visibility.Visible;
            HasTilteBar = true;

            Presenter.SetBorderAndTitleBar(true, true);

            Window.TitleBar.ExtendsContentIntoTitleBar = true;
            Window.TitleBar.SetDragRectangles(new RectInt32[] { new(40, 0, ScreenSize.Width, 48) });


            shell.SetTitleBar(titleBar);

            themeManager.Colors.Control.PropertyChanged += TextControlColorsValueChanged;
            themeManager.Colors.Text.PropertyChanged += TextControlColorsValueChanged;
            TextControlColorsValueChanged(null, new("Primary"));

            logger.Log("Set TitleBar [UIElement]");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to set TitleBar [UIElement]", ex);
            return false;
        }
    }

    public bool SetTitleBarButtonColors(
        ButtonColors backgroundColors,
        ButtonColors foregroundColors)
    {
        if (!AppWindowTitleBar.IsCustomizationSupported())
        {
            logger.Log("Failed to set TitleBar button colors", Exceptions.Unsupported);
            return false;
        }

        try
        {
            Window.TitleBar.ButtonBackgroundColor = backgroundColors.Normal;
            Window.TitleBar.ButtonHoverBackgroundColor = backgroundColors.Hover;
            Window.TitleBar.ButtonPressedBackgroundColor = backgroundColors.Pressed;
            Window.TitleBar.ButtonInactiveBackgroundColor = backgroundColors.Inactive;

            Window.TitleBar.ButtonForegroundColor = foregroundColors.Normal;
            Window.TitleBar.ButtonHoverForegroundColor = foregroundColors.Hover;
            Window.TitleBar.ButtonPressedForegroundColor = foregroundColors.Pressed;
            Window.TitleBar.ButtonInactiveForegroundColor = foregroundColors.Inactive;

            return true;
        }
        catch (Exception ex)
        {
            logger.Log("Failed to set TitleBar button colors", ex);
            return false;
        }
    }

    void TextControlColorsValueChanged(object? _, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Outline" ||
            e.PropertyName == "Primary" ||
            e.PropertyName == "Secondary" ||
            e.PropertyName == "Tertiary" ||
            e.PropertyName == "Disabled")
            SetTitleBarButtonColors(
                new(
                    Colors.Transparent,
                    themeManager.Colors.Control.Outline,
                    themeManager.Colors.Control.Primary,
                    Colors.Transparent),
                new(
                    themeManager.Colors.Text.Secondary,
                    themeManager.Colors.Text.Primary,
                    themeManager.Colors.Text.Tertiary,
                    themeManager.Colors.Text.Disabled));
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