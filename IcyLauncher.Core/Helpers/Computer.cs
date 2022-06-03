namespace IcyLauncher.Core.Helpers;

public class Computer
{
    public static string AppDataDirectory { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public static string LocalAppDataDirectory { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public static string CurrentDirectory { get; } = Environment.CurrentDirectory;

    public static string MinecraftDirectory { get; } = $"{AppDataDirectory}\\Packages\\Microsoft.MinecraftUWP_8wekyb3d8bbwe\\LocalState";
}