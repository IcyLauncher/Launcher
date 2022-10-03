using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class AcrylicBackdropHandlerView : Page
{
    public readonly AcrylicBackdropHandlerViewModel viewModel;

    public AcrylicBackdropHandlerView(AcrylicBackdropHandlerViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}