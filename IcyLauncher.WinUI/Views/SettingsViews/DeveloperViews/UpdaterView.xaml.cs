namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class UpdaterView : Page
{
    public readonly UpdaterViewModel viewModel;

    public UpdaterView(UpdaterViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}