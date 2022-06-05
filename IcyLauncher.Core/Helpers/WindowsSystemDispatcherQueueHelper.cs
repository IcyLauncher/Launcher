using System.Runtime.InteropServices;
using Windows.System;

namespace IcyLauncher.Core.Helpers;

public class WindowsSystemDispatcherQueueHelper
{
    object? dispatcherQueueController;

    public void EnsureWindowsSystemDispatcherQueueController()
    {
        if (DispatcherQueue.GetForCurrentThread() is not null || dispatcherQueueController is not null)
            return;

        Win32.DISPATCHERQUEUEOPTIONS options = new()
        {
            dwSize = Marshal.SizeOf(typeof(Win32.DISPATCHERQUEUEOPTIONS)),
            threadType = 2,
            apartmentType = 2
        };

        Win32.CreateDispatcherQueueController(options, ref dispatcherQueueController);
    }
}