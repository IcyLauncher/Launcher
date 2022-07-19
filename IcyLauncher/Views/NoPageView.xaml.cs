using IcyLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Views;

public sealed partial class NoPageView : Page
{
    public readonly NoPageViewModel viewModel = App.Provider.GetRequiredService<NoPageViewModel>();

    public NoPageView() =>
        InitializeComponent();
}