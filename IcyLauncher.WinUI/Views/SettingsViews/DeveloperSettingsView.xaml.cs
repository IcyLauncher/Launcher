using IcyLauncher.WinUI.ViewModels.SettingsViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews;

public sealed partial class DeveloperSettingsView : Page
{
    public readonly DeveloperSettingsViewModel viewModel = App.Provider.GetRequiredService<DeveloperSettingsViewModel>();

    public DeveloperSettingsView() =>
        InitializeComponent();
}