namespace IcyLauncher.Services;

public class BackdropHandler
{
    readonly WindowHandler windowHandler;
    readonly IBackdropHandler micaBackdropHandler;
    readonly IBackdropHandler acrylicBackdropHandler;
    readonly IBackdropHandler vibrancyBackdropHandler;

    public BackdropHandler(
        ILogger<BackdropHandler> logger,
        WindowHandler windowHandler,
        MicaBackdropHandler micaBackdropHandler,
        AcrylicBackdropHandler acrylicBackdropHandler,
        VibrancyBackdropHandler vibrancyBackdropHandler)
    {
        this.windowHandler = windowHandler;
        this.micaBackdropHandler = micaBackdropHandler;
        this.acrylicBackdropHandler = acrylicBackdropHandler;
        this.vibrancyBackdropHandler = vibrancyBackdropHandler;

        windowHandler.EnsureWindowsSystemDispatcherQueueController();

        logger.Log("Registered general backdrop handler");
    }


    public bool IsMicaEnabled { get; private set; }
    public bool IsAcrylicEnabled { get; private set; }
    public bool IsVibrancyEnabled { get; private set; }
    public bool IsNoneEnabled { get; private set; }
    public Backdrop? Current { get; private set; }


    public bool SetBackdrop(Backdrop backdrop, bool enable, bool? useDarkMode = null)
    {
        RemoveAllBackdrops();

        return backdrop switch
        {
            Backdrop.Mica => SetMicaBackdrop(enable, useDarkMode),
            Backdrop.Acrylic => SetAcrylicBackdrop(enable, useDarkMode),
            Backdrop.Vibrancy => SetVibrancyBackdrop(enable),
            _ => SetNoneBackdrop(enable),
        };
    }

    public void SetDarkMode(Backdrop backdrop, bool useDarkMode = true)
    {
        switch (backdrop)
        {
            case Backdrop.Mica:
                IsMicaDarkModeEnabled = useDarkMode;
                break;
            case Backdrop.Acrylic:
                IsAcrylicDarkModeEnabled = useDarkMode;
                break;
        }
    }

    void RemoveAllBackdrops()
    {
        if (IsMicaEnabled)
            SetMicaBackdrop(false);

        if (IsAcrylicEnabled)
            SetAcrylicBackdrop(false);

        if (IsVibrancyEnabled)
            SetVibrancyBackdrop(false);

        if (IsNoneEnabled)
            SetNoneBackdrop(false);
    }


    bool SetMicaBackdrop(bool enable, bool? useDarkMode = null)
    {
        var setMica = enable ? micaBackdropHandler.EnableBackdrop() : micaBackdropHandler.DisableBackdrop();
        if (useDarkMode.HasValue && IsMicaDarkModeEnabled != useDarkMode.Value)
            IsMicaDarkModeEnabled = useDarkMode.Value;

        if (setMica)
        {
            IsMicaEnabled = enable;
            Current = enable ? Backdrop.Mica : null;
        }
        return setMica;
    }

    public bool IsMicaDarkModeEnabled
    {
        get => micaBackdropHandler.IsDarkModeEnabled;
        set => micaBackdropHandler.IsDarkModeEnabled = value;
    }


    bool SetAcrylicBackdrop(bool enable, bool? useDarkMode = null)
    {
        var setAcrylic = enable ? acrylicBackdropHandler.EnableBackdrop() : acrylicBackdropHandler.DisableBackdrop();
        if (useDarkMode.HasValue && IsAcrylicDarkModeEnabled != useDarkMode.Value)
            IsAcrylicDarkModeEnabled = useDarkMode.Value;

        if (setAcrylic)
        {
            IsAcrylicEnabled = enable;
            Current = enable ? Backdrop.Acrylic : null;
        }
        return setAcrylic;
    }

    public bool IsAcrylicDarkModeEnabled
    {
        get => acrylicBackdropHandler.IsDarkModeEnabled;
        set => acrylicBackdropHandler.IsDarkModeEnabled = value;
    }


    bool SetVibrancyBackdrop(bool enable)
    {
        var setVibrancy = enable ? vibrancyBackdropHandler.EnableBackdrop() : vibrancyBackdropHandler.DisableBackdrop();

        if (setVibrancy)
        {
            IsVibrancyEnabled = enable;
            Current = enable ? Backdrop.Vibrancy : null;
        }
        return setVibrancy;
    }


    bool SetNoneBackdrop(bool enable)
    {
        var setMainBackground = windowHandler.SetMainBackground(enable ? "Background.Solid" : "Transparent");

        if (setMainBackground)
        {
            IsNoneEnabled = enable;
            Current = enable ? Backdrop.None : null;
        }
        return setMainBackground;
    }
}