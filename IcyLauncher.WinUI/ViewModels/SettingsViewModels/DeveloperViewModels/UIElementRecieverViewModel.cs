namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    public void SetupUIElementRevieverViewModel()
    {
        UIElementReviever_mainGrid = $"Hash Code: {uiElementReciever.MainGrid.GetHashCode()}";
        UIElementReviever_navigationView = $"Hash Code: {uiElementReciever.NavigationView.GetHashCode()}";
        UIElementReviever_navigationFrame = $"Hash Code: {uiElementReciever.NavigationFrame.GetHashCode()}";
        UIElementReviever_titleBarContainer = $"Hash Code: {uiElementReciever.TitleBarContainer.GetHashCode()}";
        UIElementReviever_titleBar = $"Hash Code: {uiElementReciever.TitleBar.GetHashCode()}";
        UIElementReviever_titleBarDragArea = $"Hash Code: {uiElementReciever.TitleBarDragArea.GetHashCode()}";
        UIElementReviever_backButton = $"Hash Code: {uiElementReciever.BackButton.GetHashCode()}";
        UIElementReviever_backButtonIcon = $"Hash Code: {uiElementReciever.BackButtonIcon.GetHashCode()}";
        UIElementReviever_titleBarIconGradientStops = $"Hash Code: {uiElementReciever.TitleBarIconGradientStops.GetHashCode()}";
        UIElementReviever_titleBarTitle = $"Hash Code: {uiElementReciever.TitleBarTitle.GetHashCode()}";
    }


    [ObservableProperty]
    string uIElementReviever_mainGrid = default!;

    [ObservableProperty]
    string uIElementReviever_navigationView = default!;

    [ObservableProperty]
    string uIElementReviever_navigationFrame = default!;

    [ObservableProperty]
    string uIElementReviever_titleBarContainer = default!;

    [ObservableProperty]
    string uIElementReviever_titleBar = default!;

    [ObservableProperty]
    string uIElementReviever_titleBarDragArea = default!;

    [ObservableProperty]
    string uIElementReviever_backButton = default!;

    [ObservableProperty]
    string uIElementReviever_backButtonIcon = default!;

    [ObservableProperty]
    string uIElementReviever_titleBarIconGradientStops = default!;

    [ObservableProperty]
    string uIElementReviever_titleBarTitle = default!;
}