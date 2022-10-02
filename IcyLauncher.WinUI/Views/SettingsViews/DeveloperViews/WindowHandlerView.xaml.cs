namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class WindowHandlerView : Page
{
    public readonly WindowHandlerViewModel viewModel;

    public WindowHandlerView(WindowHandlerViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}