using IcyLauncher.ViewModels;

namespace IcyLauncher.Views;

public sealed partial class ShellView : Window
{
    readonly ShellViewModel viewModel = App.Provider.GetRequiredService<ShellViewModel>();

    public ShellView()
    {
        InitializeComponent();
    }
}