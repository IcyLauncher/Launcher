using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class MicaBackdropHandlerView : Page
{
    public readonly MicaBackdropHandlerViewModel viewModel;

    public MicaBackdropHandlerView(MicaBackdropHandlerViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}