using System.Reflection;

namespace IcyLauncher.Services;

public class Updater
{
    #region Setup
    /// <summary>
    /// Service to handle and execute any new updates
    /// </summary>
    #endregion


    #region Versions
    /// <summary>
    /// Current application version
    /// </summary>
    public readonly Version CurrentAppVersion = Assembly.GetExecutingAssembly().GetName().Version is Version ver ? ver.TrimZeros() : new(0, 1);

    /// <summary>
    /// Current connected api version
    /// </summary>
    public readonly Version CurrentApiVersion = new Version(0, 0, 0, 1) is Version ver ? ver : new(0, 1);
    #endregion

    #region Information
    /// <summary>
    /// Date and time when the application last checked for any new updates
    /// </summary>
    public readonly DateTime LastChecked = DateTime.Now;

    /// <summary>
    /// Boolean whether a new update is available
    /// </summary>
    public readonly bool IsUpdateAvailable = false;
    #endregion
}