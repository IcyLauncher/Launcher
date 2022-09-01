namespace IcyLauncher.WinUI.ViewModels;

public partial class ProfilesViewModel : ObservableObject
{
    readonly MicaBackdropHandler mica;
    readonly AcrylicBackdropHandler acrylic;

    public ProfilesViewModel(
        MicaBackdropHandler mica,
        AcrylicBackdropHandler acrylic
        )
    {
        this.mica = mica;
        this.acrylic = acrylic;
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
}