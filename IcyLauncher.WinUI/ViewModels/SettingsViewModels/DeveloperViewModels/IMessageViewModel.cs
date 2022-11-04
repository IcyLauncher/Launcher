namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class IMessageViewModel : ObservableObject
{
    #region Setup
    readonly IMessage message;

    public IMessageViewModel(
        IMessage message)
    {
        this.message = message;
    }
    #endregion


    #region Example
    [ObservableProperty]
    string title = "Dialog Title";

    [ObservableProperty]
    string content = "This is a content message of a dialog";

    [ObservableProperty]
    bool await_ = false;

    [ObservableProperty]
    string close = "Close";

    [ObservableProperty]
    string primary = "";

    [ObservableProperty]
    string secondary = "";
    #endregion


    #region ShowAsync
    [RelayCommand(AllowConcurrentExecutions = true)]
    async Task ShowAAsync()
    {
        try
        {
            ContentDialogResult result = await message.ShowAsync(
                Title,
                Content,
                Await_,
                string.IsNullOrWhiteSpace(Close) ? null : Close,
                string.IsNullOrWhiteSpace(Primary) ? null : Primary,
                string.IsNullOrWhiteSpace(Secondary) ? null : Secondary);
        }
        catch (Exception ex)
        {
            await message.ShowAsync("message.ShowAsync()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region Show
    [RelayCommand(AllowConcurrentExecutions = true)]
    async Task ShowAsync()
    {
        try
        {
            message.Show(
                Title,
                Content,
                string.IsNullOrWhiteSpace(Close) ? null : Close,
                string.IsNullOrWhiteSpace(Primary) ? null : Primary,
                string.IsNullOrWhiteSpace(Secondary) ? null : Secondary);
        }
        catch (Exception ex)
        {
            await message.ShowAsync("message.Show()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion
}