using IcyLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Views;

public sealed partial class ProfilesView : Page
{
    public readonly ProfilesViewModel viewModel = App.Provider.GetRequiredService<ProfilesViewModel>();

    public ProfilesView() =>
        InitializeComponent();
}