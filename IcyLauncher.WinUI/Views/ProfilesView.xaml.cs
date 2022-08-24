namespace IcyLauncher.WinUI.Views;

public sealed partial class ProfilesView : Page
{
    public readonly ProfilesViewModel viewModel = App.Provider.GetRequiredService<ProfilesViewModel>();

    public ProfilesView() =>
        InitializeComponent();
}