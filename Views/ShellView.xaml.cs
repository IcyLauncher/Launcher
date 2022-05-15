using IcyLauncher.ViewModels;

namespace IcyLauncher;

public sealed partial class ShellView : Window
{
    readonly ShellViewModel viewModel = App.Provider.GetRequiredService<ShellViewModel>();

    public ShellView()
    {
        InitializeComponent();
    }
}