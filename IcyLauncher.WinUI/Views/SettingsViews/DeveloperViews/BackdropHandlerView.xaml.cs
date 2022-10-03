using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class BackdropHandlerView : Page
{
    public readonly BackdropHandlerViewModel viewModel;

    public BackdropHandlerView(BackdropHandlerViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}