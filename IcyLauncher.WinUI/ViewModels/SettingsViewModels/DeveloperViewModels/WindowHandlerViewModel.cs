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
    string windowHandler_hWnd;

    [RelayCommand]
    void WindowHandler_UpdateHWnd() =>
        WindowHandler_hWnd = $"0x{windowHandler.HWnd}";


    [ObservableProperty]
    bool windowHandler_hasCustomTitleBar;

    [RelayCommand]
    void WindowHandler_UpdateHasCustomTitlebar() =>
        WindowHandler_hasCustomTitleBar = windowHandler.HasCustomTitleBar;


    [ObservableProperty]
    string windowHandler_size;

    [RelayCommand]
    void WindowHandler_UpdateSize()
    {
        SizeInt32 size = windowHandler.Size;
        WindowHandler_size = $"{size.Width}x{size.Height}";
    }


    [ObservableProperty]
    string windowHandler_position;

    [RelayCommand]
    void WindowHandler_UpdatePosition()
    {
        PointInt32 position = windowHandler.Position;
        WindowHandler_position = $"X: {position.X}, Y: {position.Y}";
    }


    [ObservableProperty]
    string windowHandler_screenSize;

    [RelayCommand]
    void WindowHandler_UpdateScreenSize()
    {
        RectInt32 rect = windowHandler.ScreenSize;
        WindowHandler_screenSize = $"{rect.Width}x{rect.Height}";
    }


    [RelayCommand]
    async Task WindowHandler_SetIcon(string path)
    {
        if (!fileSystem.FileExists(path))
        {
            await message.ShowAsync("Something went wrong :(", "It looks like this path is invalid. Please verify the file exists.", closeButton: "Ok");
            return;
        }

        windowHandler.SetIcon(path);
    }


    [ObservableProperty]
    int windowHandler_sizeWidth;

    [ObservableProperty]
    int windowHandler_sizeHeight;

    [RelayCommand]
    void WindowHandler_SetSize() =>
        windowHandler.SetSize(WindowHandler_sizeWidth, WindowHandler_sizeHeight);


    [ObservableProperty]
    int windowHandler_minSizeWidth;

    [ObservableProperty]
    int windowHandler_minSizeHeight;

    [RelayCommand]
    void WindowHandler_SetMinSize() =>
        windowHandler.SetMinSize(WindowHandler_minSizeWidth, WindowHandler_minSizeHeight);


    [ObservableProperty]
    int windowHandler_positionX;

    [ObservableProperty]
    int windowHandler_positionY;

    [RelayCommand]
    void WindowHandler_SetPosition() =>
        windowHandler.SetPosition(WindowHandler_positionX, WindowHandler_positionY);

    [RelayCommand]
    void WindowHandler_SetPositionToCenter() =>
        windowHandler.SetPositionToCenter();


    [RelayCommand]
    void WindowHandler_EnsureWindowsSystemDispatcherQueueController() =>
        windowHandler.EnsureWindowsSystemDispatcherQueueController();


    [ObservableProperty]
    bool windowHandler_titleBarIsNull;

    [ObservableProperty]
    bool windowHandler_titleBarContainerIsNull;

    [RelayCommand]
    void WindowHandler_SetTitleBar() =>
        windowHandler.SetTitleBar(WindowHandler_titleBarIsNull ? null : uiElementReciever.TitleBarDragArea, WindowHandler_titleBarContainerIsNull ? null : uiElementReciever.TitleBarContainer);

}