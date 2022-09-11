using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.Services;

public class UIElementReciever
{
    public Grid MainGrid;
    public NavigationView NavigationView;
    public Frame NavigationFrame;
    public Grid TitleBarContainer;
    public StackPanel TitleBar;
    public Grid TitleBarDragArea;
    public Button BackButton;
    public AnimatedIcon BackButtonIcon;
    public GradientStopCollection TitleBarIconGradientStops;
    public TextBlock TitleBarTitle;

    public UIElementReciever(
        ILogger<UIElementReciever> logger,
        Window shell)
    {
        MainGrid = (Grid)shell.Content;
        NavigationView = (NavigationView)MainGrid.Children[1];
        NavigationFrame = (Frame)NavigationView.Content;
        TitleBarContainer = (Grid)MainGrid.Children[0];
        TitleBar = (StackPanel)TitleBarContainer.Children[0];
        TitleBarDragArea = (Grid)TitleBarContainer.Children[1];
        BackButton = (Button)TitleBar.Children[0];
        BackButtonIcon = (AnimatedIcon)((Viewbox)BackButton.Content).Child;
        TitleBarIconGradientStops = ((LinearGradientBrush)((Path)((Viewbox)TitleBar.Children[1]).Child).Fill).GradientStops;
        TitleBarTitle = (TextBlock)TitleBar.Children[2];

        logger.Log("Registered UIElement reciever and casted all UIElements");
    }
}