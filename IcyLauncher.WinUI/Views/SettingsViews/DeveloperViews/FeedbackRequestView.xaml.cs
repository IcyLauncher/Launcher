using IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class FeedbackRequestView : Page
{
    public readonly FeedbackRequestViewModel viewModel;

    public FeedbackRequestView(FeedbackRequestViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}