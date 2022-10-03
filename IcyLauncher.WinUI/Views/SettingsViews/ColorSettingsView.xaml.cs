using IcyLauncher.WinUI.ViewModels.SettingsViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews;

public sealed partial class ColorSettingsView : Page
{
    public readonly ColorSettingsViewModel viewModel = App.Provider.GetRequiredService<ColorSettingsViewModel>();

    public ColorSettingsView() =>
        InitializeComponent();
}