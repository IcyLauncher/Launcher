using IcyLauncher.WinUI.ViewModels.SettingsViewModels;
using Microsoft.UI.Xaml.Navigation;

namespace IcyLauncher.WinUI.Views.SettingsViews;

public sealed partial class BannerSettingsView : Page
{
    public readonly BannerSettingsViewModel viewModel = App.Provider.GetRequiredService<BannerSettingsViewModel>();

    public BannerSettingsView() =>
        InitializeComponent();


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        viewModel.SetCorrectIndex();
    }
}