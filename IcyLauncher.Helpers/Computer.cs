using Microsoft.Win32;
using System.Management;
using System.Reflection;

namespace IcyLauncher.Helpers;

public class Computer
{
    public static string AppDataDirectory { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public static string LocalAppDataDirectory { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public static string Desktop { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    public static string CurrentDirectory { get; } = Environment.CurrentDirectory;
    public static string MinecraftDirectory { get; } = $"{AppDataDirectory}\\Packages\\Microsoft.MinecraftUWP_8wekyb3d8bbwe\\LocalState";


    public static Version RuntimeVersion { get; } = Environment.Version;
    public static Version WinUIVersion { get; } = Assembly.Load("Microsoft.WinUI").GetName().Version ?? new();
    public static Version WindowsAppSDKVersion { get; } = Assembly.Load("Microsoft.WindowsAppRuntime.Bootstrap.Net").GetName().Version ?? new();
    public static Version OSVersion { get; } = Environment.OSVersion.Version;

    public static bool Is64 { get; } = Environment.Is64BitProcess;
    public static bool IsWindows11 { get; } = OSVersion.Build >= 22000;


    public static string? GetCPU() => 
        Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\SYSTEM\CentralProcessor\0", "ProcessorNameString", null) is string name ? 
        name.ToString().Replace("Processor", "").Replace("Six-Core", "") :
        null;

    public static string? GetGPU()
    {
        foreach (ManagementBaseObject i in new ManagementObjectSearcher(new SelectQuery("Win32_VideoController")).Get())
            return i["Caption"].ToString();
        return null;
    }

    public static string? GetRam()
    {
        foreach (ManagementBaseObject i in new ManagementObjectSearcher(new ObjectQuery("SELECT * FROM Win32_OperatingSystem")).Get())
            return Math.Round(Convert.ToDouble(i["TotalVisibleMemorySize"]) / 1048576, 2) is double size ? $"{size} GB" : null;
        return null;
    }

    public static string? GetOS() =>
        $"{Win32.BrandingFormatString("%WINDOWS_LONG%")} {Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", null)}";
}