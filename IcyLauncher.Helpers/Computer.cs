using Microsoft.Win32;
using System.Management;

namespace IcyLauncher.Helpers;

public class Computer
{
    public static string AppDataDirectory { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public static string LocalAppDataDirectory { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public static string Desktop { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    public static bool IsWindows11 { get; } = Environment.OSVersion.Version.Build >= 22000;

    public static string CurrentDirectory { get; } = Environment.CurrentDirectory;
    public static string MinecraftDirectory { get; } = $"{AppDataDirectory}\\Packages\\Microsoft.MinecraftUWP_8wekyb3d8bbwe\\LocalState";


    public static Version? CurrentMinecraftVersion
    {
        get
        {
            try
            {
                return null;
            }
            catch { return null; }
        }
    }

    public static string? CurrentCPU
    {
        get
        {
            try
            {
                return Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\SYSTEM\CentralProcessor\0", "ProcessorNameString", null) is string name
                    ? name.ToString()
                        .Replace("Processor", "")
                        .Replace("Six-Core", "") :
                    null;
            }
            catch { return null; }
        }
    }

    public static string? CurrentGPU
    {
        get
        {
            try
            {
                foreach (ManagementBaseObject i in new ManagementObjectSearcher(new SelectQuery("Win32_VideoController")).Get())
                    return i["Caption"].ToString();
                return null;
            }
            catch { return null; }
        }
    }

    public static double? CurrentRAM
    {
        get
        {
            try
            {
                foreach (ManagementBaseObject i in new ManagementObjectSearcher(new ObjectQuery("SELECT * FROM Win32_OperatingSystem")).Get())
                    return Math.Round(Convert.ToDouble(i["TotalVisibleMemorySize"]) / 1048576, 2);
                return null;
            }
            catch { return null; }
        }
    }

    public static string? CurrentOS
    {
        get
        {
            try
            {
                return $"{Win32.BrandingFormatString("%WINDOWS_LONG%")} {Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", null)}";
            }
            catch { return null; }
        }
    }
}