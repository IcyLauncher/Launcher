using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class INavigationView : Page
{
    public readonly INavigationViewModel viewModel;

    public INavigationView(INavigationViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}