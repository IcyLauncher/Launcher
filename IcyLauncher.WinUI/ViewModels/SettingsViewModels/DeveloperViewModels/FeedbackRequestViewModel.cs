namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class FeedbackRequestViewModel : ObservableObject
{
    readonly FeedbackRequest feedbackRequest;
    readonly IMessage message;
    
    public FeedbackRequestViewModel(
        FeedbackRequest feedbackRequest,
        IMessage message)
    {
        this.feedbackRequest = feedbackRequest;
        this.message = message;
    }


    [ObservableProperty]
    bool randomShouldShow;

    [RelayCommand]
    void UpdateRandomShouldShow() =>
        RandomShouldShow = feedbackRequest.RandomShouldShow;


    [RelayCommand]
    async Task ShowAsync()
    {
        try
        {
            Feedback result = await feedbackRequest.ShowAsync();
            await message.ShowAsync("feedbackRequest.ShowAsync()", $"Method completed.\nResult: {result.Result}-{result.Stars}-{result.Content}-{result.Account}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("feedbackRequest.ShowAsync()", $"Method completed.\nException{ex.Format()}");
        }
    }


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


    [ObservableProperty]
    FeedbackResult feedbackResult;

    [ObservableProperty]
    double feedbackStars;

    [ObservableProperty]
    string feedbackContent = "";

    [ObservableProperty]
    string feedbackAccount = "";
}