using Microsoft.Extensions.Options;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Runtime.InteropServices;
using Windows.Graphics;
using Windows.UI;
using WinRT.Interop;

namespace IcyLauncher.Core.Services;

public class WindowHandler
{
    readonly ILogger logger;
    readonly Configuration configuration;
    readonly Window shell;

    public WindowHandler(ILogger<Window> logger, IOptions<Configuration> configuration, Window shell)
    {
        this.logger = logger;
        this.configuration = configuration.Value;
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

    public bool IsBlurMicaEnabled { get; private set; }
    public bool IsBlurAcrylicEnabled { get; private set; }
    public bool IsBlurSimpleEnabled { get; private set; }
    public bool IsBlurNoneEnabled { get; private set; }


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
        Window.TitleBar.SetDragRectangles(new RectInt32[] { new(40, 0, ScreenSize.Width, 48) });

        Window.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        Window.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(90, 255, 255, 255);
        Window.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(50, 255, 255, 255);
        Window.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        Window.TitleBar.ButtonInactiveForegroundColor = Colors.LightGray;

        //shell.ExtendsContentIntoTitleBar = true;
        shell.SetTitleBar(titleBar);

        logger.Log("Set TitleBar to UIElement");
        return true;
    }

    public bool MakeTransparent()
    {
        try 
        {
            Win32.SubClassDelegate = new(Win32.WindowSubClass);
            var setWindowSubClass = Win32.SetWindowSubclass(HWnd, Win32.SubClassDelegate, 0, 0);
            logger.Log("Set Window Sub Class Delegate");

            long nExStyle = Win32.GetWindowLong(HWnd, -20);
            if ((nExStyle & 0x00080000) == 0)
            {
                var setWindowLong = Win32.SetWindowLong(HWnd, -20, (IntPtr)(nExStyle | 0x00080000));
                logger.Log("Set Window Long");
                var setLayeredWindowAttributes = Win32.SetLayeredWindowAttributes(HWnd, (uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Magenta), 0, 0x00000001);
                logger.Log("Set Layered Window Attributes");

                return setWindowSubClass && setWindowLong && setLayeredWindowAttributes;
            }
        }
        catch (Exception ex)
        {
            logger.Log("Failed making window transparent", ex);
        }
        return false;
    }

    public bool SetBlur(BlurEffect blurEffect, bool enable, bool useDarkMode = true)
    {
        RemoveAllBlur();

        switch (blurEffect)
        {
            case BlurEffect.Mica:
                return SetBlurMica(enable, useDarkMode);

            case BlurEffect.Acrylic:
                return SetBlurAcrylic(enable, useDarkMode);

            case BlurEffect.Simple:
                return SetBlurSimple(enable, useDarkMode);

            default:
                return SetBlurNone(enable);
        }
    }

    private void RemoveAllBlur()
    {
        if (IsBlurMicaEnabled)
            SetBlurMica(false, false);

        if (IsBlurAcrylicEnabled)
            SetBlurAcrylic(false, false);

        if (IsBlurSimpleEnabled)
            SetBlurSimple(false, false);

        if (IsBlurNoneEnabled)
            SetBlurNone(false);
    }

    private bool SetBlurMica(bool enable, bool useDarkMode)
    {
        try
        {
            var setMica = Win32.DwmSetWindowAttribute(HWnd, (int)Win32.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref enable, sizeof(int));
            logger.Log($"Set DWM Window Attribute ({setMica})");

            var setDarkMode = Win32.DwmSetWindowAttribute(HWnd, (int)Win32.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDarkMode, sizeof(int));
            logger.Log($"Set DWM Window Attribute-d ({setDarkMode})");

            if (setMica == 0)
                IsBlurMicaEnabled = enable;
            return setMica == 0 && setDarkMode == 0;
        }
        catch (Exception ex)
        {
            logger.Log($"Failed setting DWM Window Attribute ({enable})", ex);
            return false;
        }
    }

    private bool SetBlurAcrylic(bool enable, bool useDarkMode)
    {
        var setComposition = SetComposition(Win32.AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND, enable, useDarkMode);

        if (setComposition)
            IsBlurAcrylicEnabled = enable;
        return setComposition;
    }

    private bool SetBlurSimple(bool enable, bool useDarkMode)
    {
        var setComposition = SetComposition(Win32.AccentState.ACCENT_ENABLE_BLURBEHIND, enable, useDarkMode);

        if (setComposition)
            IsBlurSimpleEnabled = enable;
        return setComposition;
    }

    private bool SetComposition(Win32.AccentState state, bool enable, bool useDarkMode)
    {
        try
        {
            var policy = enable ?
            new Win32.AccentPolicy()
            {
                AccentState = state,
                GradientColor = Convert.ToUInt32(useDarkMode ? 0x990000 : 0xFFFFFF)
            } :
            new Win32.AccentPolicy()
            {
                AccentState = 0
            };
            logger.Log($"Created composition accent policy ({enable}-{useDarkMode})");

            var structSize = Marshal.SizeOf(policy);
            var ptrData = Marshal.AllocHGlobal(structSize);
            Marshal.StructureToPtr(policy, ptrData, false);
            logger.Log($"Marshaled composition struct & ptr");

            var data = new Win32.WindowCompositionAttributeData()
            {
                Attribute = Win32.WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = structSize,
                Data = ptrData
            };
            logger.Log($"Created composition data");

            var setWindowComposition = Win32.SetWindowCompositionAttribute(HWnd, ref data);
            Marshal.FreeHGlobal(ptrData);
            logger.Log($"Set Window Composition Attribute");

            return setWindowComposition;
        }
        catch (Exception ex)
        {
            logger.Log($"Failed setting Window Composition Attribute ({enable})", ex);
            return false;
        }
    }

    private bool SetBlurNone(bool enable)
    {
        try
        {
            var mainGrid = (Grid)shell.Content;
            mainGrid.Background = new SolidColorBrush(enable ? configuration.Apperance.Colors.Background.Primary : Colors.Transparent);
            logger.Log($"Set background color on MainGrid ({enable})");

            IsBlurNoneEnabled = enable;
            return true;
        }
        catch (Exception ex)
        {
            logger.Log($"Failed setting background color on MainGrid ({enable})", ex);
            return false;
        }
    }
}