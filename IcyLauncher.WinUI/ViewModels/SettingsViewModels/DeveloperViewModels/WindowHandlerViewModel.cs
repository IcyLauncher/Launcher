using Windows.Graphics;

namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class WindowHandlerViewModel : ObservableObject
{
    readonly WindowHandler windowHandler;
    readonly UIElementReciever uIElementReciever;
    readonly IMessage message;

    public WindowHandlerViewModel(
        WindowHandler windowHandler,
        UIElementReciever uIElementReciever,
        IMessage message)
    {
        this.windowHandler = windowHandler;
        this.uIElementReciever = uIElementReciever;
        this.message = message;


        UpdateHWnd();
        UpdateHasCustomTitlebar();
        UpdateSize();
        UpdatePosition();
        UpdateScreenSize();
    }


    [ObservableProperty]
    string hWnd = default!;

    [RelayCommand]
    void UpdateHWnd() =>
        HWnd = $"0x{windowHandler.HWnd}";


    [ObservableProperty]
    bool hasCustomTitleBar = default!;

    [RelayCommand]
    void UpdateHasCustomTitlebar() =>
        HasCustomTitleBar = windowHandler.HasCustomTitleBar;


    [ObservableProperty]
    string size = default!;

    [RelayCommand]
    void UpdateSize()
    {
        SizeInt32 size = windowHandler.Size;
        Size = $"{size.Width}x{size.Height}";
    }


    [ObservableProperty]
    string position = default!;

    [RelayCommand]
    void UpdatePosition()
    {
        PointInt32 position = windowHandler.Position;
        Position = $"X: {position.X}, Y: {position.Y}";
    }


    [ObservableProperty]
    string screenSize = default!;

    [RelayCommand]
    void UpdateScreenSize()
    {
        RectInt32 rect = windowHandler.ScreenSize;
        ScreenSize = $"{rect.Width}x{rect.Height}";
    }


    [RelayCommand]
    async Task SetIconAsync(
        string path)
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
    int sizeWidth;

    [ObservableProperty]
    int sizeHeight;

    [RelayCommand]
    async Task SetSizeAsync()
    {
        try
        {
            windowHandler.SetSize(SizeWidth, SizeHeight);
            await message.ShowAsync("windowHandler.SetSize()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetSize()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    int minSizeWidth;

    [ObservableProperty]
    int minSizeHeight;

    [RelayCommand]
    async Task SetMinSizeAsync()
    {
        try
        {
            windowHandler.SetMinSize(MinSizeWidth, MinSizeHeight);
            await message.ShowAsync("windowHandler.SetMinSize()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetMinSize()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [ObservableProperty]
    int positionX;

    [ObservableProperty]
    int positionY;

    [RelayCommand]
    async Task SetPositionAsync()
    {
        try
        {
            windowHandler.SetPosition(PositionX, PositionY);
            await message.ShowAsync("windowHandler.SetPosition()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetPosition()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task SetPositionToCenterAsync()
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
    async Task EnsureWindowsSystemDispatcherQueueControllerAsync()
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
    bool titleBarIsNull;

    [ObservableProperty]
    bool titleBarContainerIsNull;

    [RelayCommand]
    async Task SetTitleBarAsync()
    {
        try
        {
            bool result = windowHandler.SetTitleBar(TitleBarIsNull ? null : uIElementReciever.TitleBarDragArea, TitleBarContainerIsNull ? null : uIElementReciever.TitleBarContainer);
            await message.ShowAsync("windowHandler.SetTitleBar()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetTitleBar()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
    
    
    [ObservableProperty]
    string mainBackground = "Background.Solid";

    [RelayCommand]
    async Task SetMainBackgroundAsync()
    {
        try
        {
            bool result = windowHandler.SetMainBackground(MainBackground);
            await message.ShowAsync("windowHandler.SetMainBackground()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("windowHandler.SetMainBackground()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}