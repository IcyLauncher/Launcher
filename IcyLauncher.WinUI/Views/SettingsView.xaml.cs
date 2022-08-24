namespace IcyLauncher.WinUI.Views;

public sealed partial class SettingsView : Page
{
    public readonly SettingsViewModel viewModel = App.Provider.GetRequiredService<SettingsViewModel>();

    public SettingsView() =>
        InitializeComponent();
}