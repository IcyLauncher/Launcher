using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class VibrancyBackdropHandlerView : Page
{
    public readonly VibrancyBackdropHandlerViewModel viewModel;

    public VibrancyBackdropHandlerView(VibrancyBackdropHandlerViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}