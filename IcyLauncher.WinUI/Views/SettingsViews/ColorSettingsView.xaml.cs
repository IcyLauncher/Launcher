using IcyLauncher.WinUI.ViewModels.SettingsViewModels;
using Microsoft.UI.Xaml.Navigation;

namespace IcyLauncher.WinUI.Views.SettingsViews;

public sealed partial class ColorSettingsView : Page
{
    public readonly ColorSettingsViewModel viewModel = App.Provider.GetRequiredService<ColorSettingsViewModel>();

    public ColorSettingsView() =>
        InitializeComponent();


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        viewModel.SetCorrectIndex();
    }
}