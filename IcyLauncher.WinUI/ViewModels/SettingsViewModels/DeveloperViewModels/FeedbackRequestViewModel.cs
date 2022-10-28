namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class FeedbackRequestViewModel : ObservableObject
{
    #region Setup
    readonly FeedbackRequest feedbackRequest;
    readonly IMessage message;
    
    public FeedbackRequestViewModel(
        FeedbackRequest feedbackRequest,
        IMessage message)
    {
        this.feedbackRequest = feedbackRequest;
        this.message = message;
    }
    #endregion


    #region RandomShouldShow
    [ObservableProperty]
    bool randomShouldShow;

    [RelayCommand]
    void UpdateRandomShouldShow() =>
        RandomShouldShow = feedbackRequest.RandomShouldShow;
    #endregion


    #region ShowAsync
    [RelayCommand]
    async Task ShowAsync(bool forceShow)
    {
        try
        {
            Feedback result = await feedbackRequest.ShowAsync(forceShow);
            await message.ShowAsync("feedbackRequest.ShowAsync()", $"Method completed.\nResult: {result.Result}-{result.Stars}-{result.Content}-{result.Account}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("feedbackRequest.ShowAsync()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region SubmitAsync
    [RelayCommand]
    async Task SubmitAsync()
    {
        try
        {
            bool result = await feedbackRequest.SubmitAsync(new() { Result = FeedbackResult, Stars = FeedbackStars, Content = FeedbackContent, Account = FeedbackAccount });
            await message.ShowAsync("feedbackRequest.SubmitAsync()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("feedbackRequest.SubmitAsync()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region Example
    [ObservableProperty]
    FeedbackResult feedbackResult;

    [ObservableProperty]
    double feedbackStars;

    [ObservableProperty]
    string feedbackContent = "";

    [ObservableProperty]
    string feedbackAccount = "";
    #endregion
}