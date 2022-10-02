namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class IConverterView : Page
{
    public readonly IConverterViewModel viewModel;

    public IConverterView(IConverterViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}