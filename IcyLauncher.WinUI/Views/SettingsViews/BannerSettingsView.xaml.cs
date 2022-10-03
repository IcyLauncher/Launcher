using IcyLauncher.WinUI.ViewModels.SettingsViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews;

public sealed partial class BannerSettingsView : Page
{
    public readonly BannerSettingsViewModel viewModel = App.Provider.GetRequiredService<BannerSettingsViewModel>();

    public BannerSettingsView() =>
        InitializeComponent();
}