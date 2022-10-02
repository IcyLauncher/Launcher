namespace IcyLauncher.WinUI.Views.SettingsViews.DeveloperViews;

public sealed partial class ThemeManagerView : Page
{
    public readonly ThemeManagerViewModel viewModel;

    public ThemeManagerView(ThemeManagerViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }
}