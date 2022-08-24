namespace IcyLauncher.WinUI.Views;

public sealed partial class DeveloperSettingsView : Page
{
    public readonly DeveloperSettingsViewModel viewModel = App.Provider.GetRequiredService<DeveloperSettingsViewModel>();

    public DeveloperSettingsView() =>
        InitializeComponent();
}