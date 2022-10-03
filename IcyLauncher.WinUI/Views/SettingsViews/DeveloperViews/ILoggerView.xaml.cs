using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class ILoggerView : Page
{
    public readonly ILoggerViewModel viewModel;

    public ILoggerView(ILoggerViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}