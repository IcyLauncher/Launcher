using IcyLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Views;

public sealed partial class DeveloperSettingsView : Page
{
    public readonly DeveloperSettingsViewModel viewModel = App.Provider.GetRequiredService<DeveloperSettingsViewModel>();

    public DeveloperSettingsView() =>
        InitializeComponent();
}