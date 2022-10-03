using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class IFileSystemView : Page
{
    public readonly IFileSystemViewModel viewModel;

    public IFileSystemView(IFileSystemViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}