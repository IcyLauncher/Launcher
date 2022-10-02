namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class IMessageView : Page
{
    public readonly IMessageViewModel viewModel;

    public IMessageView(IMessageViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}