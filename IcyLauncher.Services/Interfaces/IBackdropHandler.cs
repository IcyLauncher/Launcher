namespace IcyLauncher.Services.Interfaces;

public interface IBackdropHandler
{
    bool EnableBackdrop();

    bool DisableBackdrop();

    bool IsDarkModeEnabled { get; set; }
}