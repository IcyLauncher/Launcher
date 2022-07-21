using System.Reflection;

namespace IcyLauncher.Core.Services;

public class Updater
{
    public readonly Version CurrentAppVersion = Assembly.GetExecutingAssembly().GetName().Version is Version ver ? ver.TrimZeros() : new(0, 1);
    public readonly Version CurrentApiVersion = new Version(0, 0, 0, 1) is Version ver ? ver : new(0, 1);

    public readonly DateTime LastChecked = DateTime.Now;

    public readonly bool IsUpdateAvailable = false;
}