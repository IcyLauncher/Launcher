namespace IcyLauncher.WinUI.Views;

public sealed partial class NoPageView : Page
{
    public readonly NoPageViewModel viewModel = App.Provider.GetRequiredService<NoPageViewModel>();

    public NoPageView() =>
        InitializeComponent();
}