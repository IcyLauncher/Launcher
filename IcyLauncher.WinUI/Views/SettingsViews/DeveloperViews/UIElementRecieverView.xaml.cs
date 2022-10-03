using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class UIElementRecieverView : Page
{
    public readonly UIElementRecieverViewModel viewModel;

    public UIElementRecieverView(UIElementRecieverViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}