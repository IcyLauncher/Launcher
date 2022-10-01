namespace IcyLauncher.Services.Interfaces;

public interface IBackdropHandler
{
    /// <summary>
    /// Enables the backdrop
    /// </summary>
    /// <returns>A boolean wether the backdrop was enabled successfully</returns>
    bool EnableBackdrop();

    /// <summary>
    /// Disables the backdrop
    /// </summary>
    /// <returns>A boolean wether the backdrop was disabled successfully</returns>
    bool DisableBackdrop();


    /// <summary>
    /// Not all BackdropHandlers implement a light/dark mode. This boolean wont affect some system backdrops like vibrancy
    /// </summary>
    bool IsDarkModeEnabled { get; set; }
}