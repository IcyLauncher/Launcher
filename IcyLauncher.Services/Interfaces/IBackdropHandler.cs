namespace IcyLauncher.Services.Interfaces;

public interface IBackdropHandler
{
    bool EnableBackdrop();

    bool DisableBackdrop();


    /// <summary>
    /// Not all BackdropHandlers implement a light/dark mode. This boolean wont affect some system backdrops like vibrancy.
    /// </summary>
    bool IsDarkModeEnabled { get; set; }
}