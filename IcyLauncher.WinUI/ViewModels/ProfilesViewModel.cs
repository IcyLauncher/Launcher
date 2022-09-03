namespace IcyLauncher.WinUI.ViewModels;

public partial class ProfilesViewModel : ObservableObject
{
    readonly IBackdropHandler mica;
    readonly IBackdropHandler acrylic;
    readonly IBackdropHandler vibrancy;

    public ProfilesViewModel(
        MicaBackdropHandler mica,
        AcrylicBackdropHandler acrylic,
        VibrancyBackdropHandler vibrancy
        )
    {
        this.mica = mica;
        this.acrylic = acrylic;
        this.vibrancy = vibrancy;
    }


    [RelayCommand]
    void EnableMica()
    {
        mica.EnableBackdrop();
    }

    [RelayCommand]
    void DisableMica()
    {
        mica.DisableBackdrop();
    }

    [ObservableProperty]
    bool isMicaDark = true;

    partial void OnIsMicaDarkChanged(bool value)
    {
        mica.IsDarkModeEnabled = value;
    }


    [RelayCommand]
    void EnableAcrylic()
    {
        acrylic.EnableBackdrop();
    }

    [RelayCommand]
    void DisableAcrliyc()
    {
        acrylic.DisableBackdrop();
    }

    [ObservableProperty]
    bool isAcrylicDark = true;

    partial void OnIsAcrylicDarkChanged(bool value)
    {
        acrylic.IsDarkModeEnabled = value;
    }


    [RelayCommand]
    void EnableVibrancy()
    {
        vibrancy.EnableBackdrop();
    }

    [RelayCommand]
    void DisableVibrancy()
    {
        vibrancy.DisableBackdrop();
    }

    [ObservableProperty]
    bool isVibrancyDark = true;

    partial void OnIsVibrancyDarkChanged(bool value)
    {
        vibrancy.IsDarkModeEnabled = value;
    }
}