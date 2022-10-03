using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class ImagingUtilityView : Page
{
    public readonly ImagingUtilityViewModel viewModel;

    public ImagingUtilityView(ImagingUtilityViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}