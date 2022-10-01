﻿namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    void SetupBackdropHandlerViewModel()
    {
        BackdropHandler_UpdateIsMicaEnabled();
        BackdropHandler_UpdateIsAcrylicEnabled();
        BackdropHandler_UpdateIsVibrancyEnabled();
        BackdropHandler_UpdateIsNoneEnabled();
        BackdropHandler_UpdateCurrent();
    }


    [ObservableProperty]
    bool backdropHandler_isMicaEnabled = default!;

    [RelayCommand]
    void BackdropHandler_UpdateIsMicaEnabled() =>
        BackdropHandler_isMicaEnabled = backdropHandler.IsMicaEnabled;


    [ObservableProperty]
    bool backdropHandler_isAcrylicEnabled = default!;

    [RelayCommand]
    void BackdropHandler_UpdateIsAcrylicEnabled() =>
        BackdropHandler_isAcrylicEnabled = backdropHandler.IsAcrylicEnabled;


    [ObservableProperty]
    bool backdropHandler_isVibrancyEnabled = default!;

    [RelayCommand]
    void BackdropHandler_UpdateIsVibrancyEnabled() =>
        BackdropHandler_isVibrancyEnabled = backdropHandler.IsVibrancyEnabled;


    [ObservableProperty]
    bool backdropHandler_isNoneEnabled = default!;

    [RelayCommand]
    void BackdropHandler_UpdateIsNoneEnabled() =>
        BackdropHandler_isNoneEnabled = backdropHandler.IsNoneEnabled;


    [ObservableProperty]
    Backdrop? backdropHandler_current = default!;

    [RelayCommand]
    void BackdropHandler_UpdateCurrent() =>
        BackdropHandler_current = backdropHandler.Current;

    
    [ObservableProperty]
    Backdrop backdropHandler_backdrop;
    
    [ObservableProperty]
    bool backdropHandler_enable = true;
    
    [ObservableProperty]
    bool backdropHandler_useDarkMode = true;
    
    [ObservableProperty]
    bool backdropHandler_useDarkModeIsNull;

    [RelayCommand]
    async Task BackdropHandler_SetBackdrop()
    {
        try
        {
            bool result = backdropHandler.SetBackdrop(BackdropHandler_backdrop, BackdropHandler_enable, BackdropHandler_useDarkModeIsNull ? null : BackdropHandler_useDarkMode);
            await message.ShowAsync("backdropHandler.SetBackdrop()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("backdropHandler.SetBackdrop()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task BackdropHandler_SetDarkMode()
    {
        try
        {
            backdropHandler.SetDarkMode(BackdropHandler_backdrop, BackdropHandler_useDarkMode);
            await message.ShowAsync("backdropHandler.SetDarkMode()", $"Method completed", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("backdropHandler.SetDarkMode()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}