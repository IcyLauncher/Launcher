using IcyLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Views;

public sealed partial class BannerSettingsView : Page
{
    public readonly BannerSettingsViewModel viewModel = App.Provider.GetRequiredService<BannerSettingsViewModel>();

    public BannerSettingsView() =>
        InitializeComponent();
}