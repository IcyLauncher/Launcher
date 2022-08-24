#pragma warning disable CA1401
#pragma warning disable CA2211
#pragma warning disable IDE0060

using System.Runtime.InteropServices;

namespace IcyLauncher.Helpers;

public class Win32
{
    public delegate IntPtr WinProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    public delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData);

    public static WinProc? NewWndProc { get; set; }
    public static SUBCLASSPROC? SubClassDelegate { get; set; }
    public static IntPtr OldWndProc { get; set; } = IntPtr.Zero;

    public static int MinWidth = 0;
    public static int MinHeight = 0;


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPATCHERQUEUEOPTIONS
    {
        public int dwSize;
        public int threadType;
        public int apartmentType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(int Left, int Top, int Right, int Bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }

        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public enum DWMWINDOWATTRIBUTE
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        DWMWA_MICA_EFFECT = 1029
    };

    public enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AccentPolicy
    {
        public AccentState AccentState;
        public uint AccentFlags;
        public uint GradientColor;
        public uint AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    public enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19
    }


    [DllImport("user32.dll")]
    public static extern IntPtr GetDpiForWindow(IntPtr hwnd);

    [DllImport("user32")]
    public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, WinProc newProc);

    [DllImport("user32.dll")]
    public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("CoreMessaging.dll")]
    public static extern IntPtr CreateDispatcherQueueController([In] DISPATCHERQUEUEOPTIONS options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object? dispatcherQueueController);

    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool FillRect(IntPtr hdc, [In] ref RECT rect, IntPtr hbrush);

    [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr CreateSolidBrush(int crColor);

    [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool DeleteObject([In] IntPtr hObject);

    [DllImport("Comctl32.dll", SetLastError = true)]
    public static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, uint uIdSubclass, uint dwRefData);

    [DllImport("Comctl32.dll", SetLastError = true)]
    public static extern int DefSubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

    [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
    public static extern bool SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
    public static extern bool SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("User32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
    public static extern long GetWindowLong32(IntPtr hWnd, int nIndex);

    [DllImport("User32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
    public static extern long GetWindowLongPtr64(IntPtr hWnd, int nIndex);

    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("dwmapi.dll", PreserveSig = true)]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref bool attrValue, int attrSize);

    [DllImport("user32.dll")]
    public static extern bool SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    [DllImport("winbrand.dll", CharSet = CharSet.Unicode)]
    public static extern string BrandingFormatString(string format);


    public static IntPtr NewWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == 36)
        {
            MINMAXINFO minMaxInfo = Marshal.PtrToStructure<MINMAXINFO>(lParam);
            minMaxInfo.ptMinTrackSize.x = MinWidth;
            minMaxInfo.ptMinTrackSize.y = MinHeight;
            Marshal.StructureToPtr(minMaxInfo, lParam, true);

        }
        return CallWindowProc(OldWndProc, hWnd, msg, wParam, lParam);
    }

    public static int WindowSubClass(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData)
    {
        switch (uMsg)
        {
            case 0x0014:
                {
                    GetClientRect(hWnd, out RECT rect);
                    rect.right = 0;

                    IntPtr hBrush = CreateSolidBrush(System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Magenta));

                    FillRect(wParam, ref rect, hBrush);
                    DeleteObject(hBrush);
                    return 1;
                }
        }

        return DefSubclassProc(hWnd, uMsg, wParam, lParam);
    }

    public static bool SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong) =>
        IntPtr.Size == 4 ? SetWindowLongPtr32(hWnd, nIndex, dwNewLong) : SetWindowLongPtr64(hWnd, nIndex, dwNewLong);

    public static long GetWindowLong(IntPtr hWnd, int nIndex) =>
        IntPtr.Size == 4 ? GetWindowLong32(hWnd, nIndex) : GetWindowLongPtr64(hWnd, nIndex);
}