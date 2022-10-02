namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class ConfigurationManagerView : Page
{
    public readonly ConfigurationManagerViewModel viewModel;

    public ConfigurationManagerView(ConfigurationManagerViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}