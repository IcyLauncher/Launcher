using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.Services;

public class UIElementReciever
{
    /// <summary>
    /// The container of the current main window
    /// </summary>
    public readonly Grid MainGrid;
    /// <summary>
    /// The navigation container of the current main window
    /// </summary>
    public readonly NavigationView NavigationView;
    /// <summary>
    /// The navigation frame of the current main window
    /// </summary>
    public readonly Frame NavigationFrame;
    /// <summary>
    /// The container of the custom title bar
    /// </summary>
    public readonly Grid TitleBarContainer;
    /// <summary>
    /// The custom title bar of the current main window
    /// </summary>
    public readonly StackPanel TitleBar;
    /// <summary>
    /// The drag area of the custom title bar
    /// </summary>
    public readonly Grid TitleBarDragArea;
    /// <summary>
    /// The navigation back button of the navigation view
    /// </summary>
    public readonly Button BackButton;
    /// <summary>
    /// The icon of the back button
    /// </summary>
    public readonly AnimatedIcon BackButtonIcon;
    /// <summary>
    /// The icon gradient stop collection of the custom title bar
    /// </summary>
    public readonly GradientStopCollection TitleBarIconGradientStops;
    /// <summary>
    /// The title of the custom title bar
    /// </summary>
    public readonly TextBlock TitleBarTitle;

    /// <summary>
    /// Reciever and caster of all UIElements of the current main window
    /// </summary>
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