using IcyLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Views;

public sealed partial class SettingsView : Page
{
    public readonly SettingsViewModel viewModel = App.Provider.GetRequiredService<SettingsViewModel>();

    public SettingsView() =>
        InitializeComponent();
}