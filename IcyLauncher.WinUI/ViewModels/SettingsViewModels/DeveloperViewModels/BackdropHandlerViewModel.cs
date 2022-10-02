﻿namespace IcyLauncher.WinUI.ViewModels;

public partial class BackdropHandlerViewModel : ObservableObject
{
    readonly BackdropHandler backdropHandler;
    readonly IMessage message;

    public BackdropHandlerViewModel(
        BackdropHandler backdropHandler,
        IMessage message)
    {
        this.backdropHandler = backdropHandler;
        this.message = message;


        UpdateIsMicaEnabled();
        UpdateIsAcrylicEnabled();
        UpdateIsVibrancyEnabled();
        UpdateIsNoneEnabled();
        UpdateCurrent();
    }


    [ObservableProperty]
    bool isMicaEnabled = default!;

    [RelayCommand]
    void UpdateIsMicaEnabled() =>
        IsMicaEnabled = backdropHandler.IsMicaEnabled;


    [ObservableProperty]
    bool isAcrylicEnabled = default!;

    [RelayCommand]
    void UpdateIsAcrylicEnabled() =>
        IsAcrylicEnabled = backdropHandler.IsAcrylicEnabled;


    [ObservableProperty]
    bool isVibrancyEnabled = default!;

    [RelayCommand]
    void UpdateIsVibrancyEnabled() =>
        IsVibrancyEnabled = backdropHandler.IsVibrancyEnabled;


    [ObservableProperty]
    bool isNoneEnabled = default!;

    [RelayCommand]
    void UpdateIsNoneEnabled() =>
        IsNoneEnabled = backdropHandler.IsNoneEnabled;


    [ObservableProperty]
    Backdrop? current = default!;

    [RelayCommand]
    void UpdateCurrent() =>
        Current = backdropHandler.Current;

    
    [ObservableProperty]
    Backdrop backdrop;
    
    [ObservableProperty]
    bool enable = true;
    
    [ObservableProperty]
    bool useDarkMode = true;
    
    [ObservableProperty]
    bool useDarkModeIsNull;

    [RelayCommand]
    async Task SetBackdropAsync()
    {
        try
        {
            bool result = backdropHandler.SetBackdrop(Backdrop, Enable, UseDarkModeIsNull ? null : UseDarkMode);
            await message.ShowAsync("backdropHandler.SetBackdrop()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("backdropHandler.SetBackdrop()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task SetDarkModeAsync()
    {
        try
        {
            backdropHandler.SetDarkMode(Backdrop, UseDarkMode);
            await message.ShowAsync("backdropHandler.SetDarkMode()", $"Method completed", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("backdropHandler.SetDarkMode()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}