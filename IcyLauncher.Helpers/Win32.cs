#pragma warning disable CA1401
#pragma warning disable CA2211

using System.Runtime.InteropServices;

namespace IcyLauncher.Helpers;

public class Win32
{
    public delegate IntPtr WinProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    public delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData);

    public static WinProc? NewWndProc { get; set; }
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


    [DllImport("user32.dll")]
    public static extern IntPtr GetDpiForWindow(IntPtr hwnd);

    [DllImport("user32")]
    public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, WinProc newProc);

    [DllImport("user32.dll")]
    public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("CoreMessaging.dll")]
    public static extern IntPtr CreateDispatcherQueueController([In] DISPATCHERQUEUEOPTIONS options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object? dispatcherQueueController);

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
}