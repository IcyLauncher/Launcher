namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class HomeView : Page
{
    public readonly ViewModels.SettingsViewModels.DeveloperViewModels.HomeViewModel viewModel;

    public HomeView(ViewModels.SettingsViewModels.DeveloperViewModels.HomeViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}