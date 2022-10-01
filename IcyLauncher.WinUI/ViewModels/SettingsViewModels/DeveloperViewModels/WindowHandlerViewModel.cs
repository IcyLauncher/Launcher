using Windows.Graphics;

namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    void SetupWindowHandlerViewModel()
    {
        WindowHandler_UpdateHWnd();
        WindowHandler_UpdateHasCustomTitlebar();
        WindowHandler_UpdateSize();
        WindowHandler_UpdatePosition();
        WindowHandler_UpdateScreenSize();
    }


    [ObservableProperty]
    string windowHandler_hWnd = default!;

    [RelayCommand]
    void WindowHandler_UpdateHWnd() =>
        WindowHandler_hWnd = $"0x{windowHandler.HWnd}";


    [ObservableProperty]
    bool windowHandler_hasCustomTitleBar = default!;

    [RelayCommand]
    void WindowHandler_UpdateHasCustomTitlebar() =>
        WindowHandler_hasCustomTitleBar = windowHandler.HasCustomTitleBar;


    [ObservableProperty]
    string windowHandler_size = default!;

    [RelayCommand]
    void WindowHandler_UpdateSize()
    {
        SizeInt32 size = windowHandler.Size;
        WindowHandler_size = $"{size.Width}x{size.Height}";
    }


    [ObservableProperty]
    string windowHandler_position = default!;

    [RelayCommand]
    void WindowHandler_UpdatePosition()
    {
        PointInt32 position = windowHandler.Position;
        WindowHandler_position = $"X: {position.X}, Y: {position.Y}";
    }


    [ObservableProperty]
    string windowHandler_screenSize = default!;

    [RelayCommand]
    void WindowHandler_UpdateScreenSize()
    {
        RectInt32 rect = windowHandler.ScreenSize;
        WindowHandler_screenSize = $"{rect.Width}x{rect.Height}";
    }


    [RelayCommand]
    async Task WindowHandler_SetIcon(string path)
    {
        try
        {
            windowHandler.SetIcon(path);
            await message.ShowAsync("windowHandler.SetIcon()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetIcon()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    int windowHandler_sizeWidth;

    [ObservableProperty]
    int windowHandler_sizeHeight;

    [RelayCommand]
    async Task WindowHandler_SetSize()
    {
        try
        {
            windowHandler.SetSize(WindowHandler_sizeWidth, WindowHandler_sizeHeight);
            await message.ShowAsync("windowHandler.SetSize()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetSize()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    int windowHandler_minSizeWidth;

    [ObservableProperty]
    int windowHandler_minSizeHeight;

    [RelayCommand]
    async Task WindowHandler_SetMinSize()
    {
        try
        {
            windowHandler.SetMinSize(WindowHandler_minSizeWidth, WindowHandler_minSizeHeight);
            await message.ShowAsync("windowHandler.SetMinSize()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetMinSize()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    int windowHandler_positionX;

    [ObservableProperty]
    int windowHandler_positionY;

    [RelayCommand]
    async Task WindowHandler_SetPosition()
    {
        try
        {
            windowHandler.SetPosition(WindowHandler_positionX, WindowHandler_positionY);
            await message.ShowAsync("windowHandler.SetPosition()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetPosition()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task WindowHandler_SetPositionToCenter()
    {
        try
        {
            windowHandler.SetPositionToCenter();
            await message.ShowAsync("windowHandler.SetPositionToCenter()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetPositionToCenter()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [RelayCommand]
    async Task WindowHandler_EnsureWindowsSystemDispatcherQueueController()
    {
        try
        {
            bool result = windowHandler.EnsureWindowsSystemDispatcherQueueController();
            await message.ShowAsync("windowHandler.EnsureWindowsSystemDispatcherQueueController()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.EnsureWindowsSystemDispatcherQueueController()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    bool windowHandler_titleBarIsNull;

    [ObservableProperty]
    bool windowHandler_titleBarContainerIsNull;

    [RelayCommand]
    async Task WindowHandler_SetTitleBar()
    {
        try
        {
            bool result = windowHandler.SetTitleBar(WindowHandler_titleBarIsNull ? null : uiElementReciever.TitleBarDragArea, WindowHandler_titleBarContainerIsNull ? null : uiElementReciever.TitleBarContainer);
            await message.ShowAsync("windowHandler.SetTitleBar()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetTitleBar()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
    
    
    [ObservableProperty]
    string windowHandler_mainBackground = "Background.Solid";

    [RelayCommand]
    async Task WindowHandler_SetMainBackground()
    {
        try
        {
            bool result = windowHandler.SetMainBackground(WindowHandler_mainBackground);
            await message.ShowAsync("windowHandler.SetMainBackground()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetMainBackground()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}