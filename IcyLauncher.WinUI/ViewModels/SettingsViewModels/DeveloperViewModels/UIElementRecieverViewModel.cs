﻿namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class UIElementRecieverViewModel : ObservableObject
{
    #region Setup
    public UIElementRecieverViewModel(
        UIElementReciever uIElementReciever)
    {
        MainGrid = $"Hash Code: {uIElementReciever.MainGrid.GetHashCode()}";
        NavigationView = $"Hash Code: {uIElementReciever.NavigationView.GetHashCode()}";
        NavigationFrame = $"Hash Code: {uIElementReciever.NavigationFrame.GetHashCode()}";
        TitleBarContainer = $"Hash Code: {uIElementReciever.TitleBarContainer.GetHashCode()}";
        TitleBar = $"Hash Code: {uIElementReciever.TitleBar.GetHashCode()}";
        TitleBarDragArea = $"Hash Code: {uIElementReciever.TitleBarDragArea.GetHashCode()}";
        BackButton = $"Hash Code: {uIElementReciever.BackButton.GetHashCode()}";
        BackButtonIcon = $"Hash Code: {uIElementReciever.BackButtonIcon.GetHashCode()}";
        TitleBarIconGradientStops = $"Hash Code: {uIElementReciever.TitleBarIconGradientStops.GetHashCode()}";
        TitleBarTitle = $"Hash Code: {uIElementReciever.TitleBarTitle.GetHashCode()}";
    }
    #endregion


    #region UIElements
    [ObservableProperty]
    string mainGrid = default!;

    [ObservableProperty]
    string navigationView = default!;

    [ObservableProperty]
    string navigationFrame = default!;

    [ObservableProperty]
    string titleBarContainer = default!;

    [ObservableProperty]
    string titleBar = default!;

    [ObservableProperty]
    string titleBarDragArea = default!;

    [ObservableProperty]
    string backButton = default!;

    [ObservableProperty]
    string backButtonIcon = default!;

    [ObservableProperty]
    string titleBarIconGradientStops = default!;

    [ObservableProperty]
    string titleBarTitle = default!;
    #endregion
}