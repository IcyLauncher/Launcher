namespace IcyLauncher.Services;

public class BackdropHandler
{
    #region Setup
    readonly WindowHandler windowHandler;
    readonly IBackdropHandler micaBackdropHandler;
    readonly IBackdropHandler acrylicBackdropHandler;
    readonly IBackdropHandler vibrancyBackdropHandler;

    /// <summary>
    /// Handler to configure all backdrop effects on the current main window
    /// </summary>
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
    #endregion


    #region States
    /// <summary>
    /// Boolean wether a mica backdrop effect is enabled
    /// </summary>
    public bool IsMicaEnabled { get; private set; }

    /// <summary>
    /// Boolean wether an acrylic backdrop effect is enabled
    /// </summary>
    public bool IsAcrylicEnabled { get; private set; }

    /// <summary>
    /// Boolean wether a vivrancy backdrop effect is enabled
    /// </summary>
    public bool IsVibrancyEnabled { get; private set; }

    /// <summary>
    /// Boolean wether no backdrop effect is enabled
    /// </summary>
    public bool IsNoneEnabled { get; private set; }

    /// <summary>
    /// The currently activated backdrop effect
    /// </summary>
    public Backdrop? Current { get; private set; }
    #endregion

    #region General
    /// <summary>
    /// Enables/Disables a backdrop effect on the current main window and optionally updates the dark mode
    /// </summary>
    /// <param name="backdrop">The backdrop effect to enable/disable</param>
    /// <param name="enable">The boolean wether the backdrop effect should be enabled/disabled</param>
    /// <param name="useDarkMode">The boolean wether dark mode should be actived</param>
    /// <returns>A boolean wether the backdrop effect set successfully</returns>
    public bool SetBackdrop(
        Backdrop backdrop,
        bool enable,
        bool? useDarkMode = null)
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

    /// <summary>
    /// Sets wether dark mode should be actived on the given backdrop effect
    /// </summary>
    /// <param name="backdrop">The backdrop effect to set the dark mode</param>
    /// <param name="useDarkMode">The boolean wether the backdrop effect should be enabled/disabled</param>
    public void SetDarkMode(
        Backdrop backdrop,
        bool useDarkMode = true)
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

    /// <summary>
    /// Removes all backdrop effects
    /// </summary>
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
    #endregion


    #region Mica
    /// <summary>
    /// Enables/Disable a mica backdrop effect
    /// </summary>
    /// <param name="enable">The boolean wether the backdrop effect should be enabled/disabled</param>
    /// <param name="useDarkMode">The boolean wether dark mode should be actived</param>
    /// <returns>A boolean wether the backdrop effect set successfully</returns>
    bool SetMicaBackdrop(
        bool enable,
        bool? useDarkMode = null)
    {
        bool setMica = enable ? micaBackdropHandler.EnableBackdrop() : micaBackdropHandler.DisableBackdrop();
        if (useDarkMode.HasValue && IsMicaDarkModeEnabled != useDarkMode.Value)
            IsMicaDarkModeEnabled = useDarkMode.Value;

        if (setMica)
        {
            IsMicaEnabled = enable;
            Current = enable ? Backdrop.Mica : null;
        }
        return setMica;
    }

    /// <summary>
    /// Gets and sets the dark mode of the mica backdrop effect
    /// /// </summary>
    public bool IsMicaDarkModeEnabled
    {
        get => micaBackdropHandler.IsDarkModeEnabled;
        set => micaBackdropHandler.IsDarkModeEnabled = value;
    }
    #endregion

    #region Acrylic
    /// <summary>
    /// Enables/Disable an acrylic backdrop effect
    /// </summary>
    /// <param name="enable">The boolean wether the backdrop effect should be enabled/disabled</param>
    /// <param name="useDarkMode">The boolean wether dark mode should be actived</param>
    /// <returns>A boolean wether the backdrop effect set successfully</returns>
    bool SetAcrylicBackdrop(
        bool enable,
        bool? useDarkMode = null)
    {
        bool setAcrylic = enable ? acrylicBackdropHandler.EnableBackdrop() : acrylicBackdropHandler.DisableBackdrop();
        if (useDarkMode.HasValue && IsAcrylicDarkModeEnabled != useDarkMode.Value)
            IsAcrylicDarkModeEnabled = useDarkMode.Value;

        if (setAcrylic)
        {
            IsAcrylicEnabled = enable;
            Current = enable ? Backdrop.Acrylic : null;
        }
        return setAcrylic;
    }

    /// <summary>
    /// Gets and sets the dark mode of the acrliyc backdrop effect
    /// /// </summary>
    public bool IsAcrylicDarkModeEnabled
    {
        get => acrylicBackdropHandler.IsDarkModeEnabled;
        set => acrylicBackdropHandler.IsDarkModeEnabled = value;
    }
    #endregion

    #region Vibrancy
    /// <summary>
    /// Enables/Disable a vibrancy backdrop effect
    /// </summary>
    /// <param name="enable">The boolean wether the backdrop effect should be enabled/disabled</param>
    /// <returns>A boolean wether the backdrop effect set successfully</returns>
    bool SetVibrancyBackdrop(
        bool enable)
    {
        bool setVibrancy = enable ? vibrancyBackdropHandler.EnableBackdrop() : vibrancyBackdropHandler.DisableBackdrop();

        if (setVibrancy)
        {
            IsVibrancyEnabled = enable;
            Current = enable ? Backdrop.Vibrancy : null;
        }
        return setVibrancy;
    }
    #endregion

    #region None
    /// <summary>
    /// Enables/Disable no backdrop effect
    /// </summary>
    /// <param name="enable">The boolean wether no backdrop effect should be enabled/disabled</param>
    /// <returns>A boolean wether no backdrop effect set successfully</returns>
    bool SetNoneBackdrop(
        bool enable)
    {
        bool setMainBackground = windowHandler.SetMainBackground(enable ? "Background.Solid" : "Transparent");

        if (setMainBackground)
        {
            IsNoneEnabled = enable;
            Current = enable ? Backdrop.None : null;
        }
        return setMainBackground;
    }
    #endregion
}