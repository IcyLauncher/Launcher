namespace IcyLauncher.WinUI.Views;

public sealed partial class HomeView : Page
{
    public readonly HomeViewModel viewModel = App.Provider.GetRequiredService<HomeViewModel>();

    public HomeView() =>
        InitializeComponent();
}