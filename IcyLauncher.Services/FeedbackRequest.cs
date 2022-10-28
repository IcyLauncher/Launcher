using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace IcyLauncher.Services;

public class FeedbackRequest
{
    #region Setup
    readonly Configuration configuration;
    readonly ILogger logger;
    readonly IMessage message;

    /// <summary>
    /// Service to request user feedback and submit it to the connected API
    /// </summary>
    public FeedbackRequest(
        IOptions<Configuration> configuration,
        ILogger<FeedbackRequest> logger,
        IMessage message)
    {
        this.configuration = configuration.Value;
        this.logger = logger;
        this.message = message;
    }
    #endregion


    #region Randomization
    readonly Random random = new();

    /// <returns>A boolean with the chance of 1/20 to be true</returns>
    public bool RandomShouldShow => random.Next(20) == 0;
    #endregion


    #region Actions
    /// <summary>
    /// Requests a new user feedback popup
    /// </summary>
    /// <param name="forceShow">Wether to force request even if AskForFeedback is set to NeverShowAgain</param>
    /// <returns>The feedback result with all infortmation</returns>
    public async Task<Feedback> ShowAsync(bool forceShow = false)
    {
        if (!forceShow && !configuration.Launcher.AskForFeedback)
        {
            logger.Log($"Tried to show feedback request [NeverShowAgain]");
            return new() { Result = FeedbackResult.NeverShowAgain };
        }

        StackPanel feedback = UIElementProvider.FeedbackContainer(
            out RatingControl rating,
            out TextBox content);

        ContentDialog dialog = new()
        {
            Content = feedback,
            CloseButtonText = "Cancel",
            PrimaryButtonText = "Submit",
            SecondaryButtonText = "Never show again :(",
            IsPrimaryButtonEnabled = false
        };
        rating.ValueChanged += (s, e) => dialog.IsPrimaryButtonEnabled = rating.Value != -1;
        ContentDialogResult request = await message.ShowAsync(dialog);

        Feedback result = new();
        result.Result = request switch
        {
            ContentDialogResult.Primary => FeedbackResult.Submit,
            ContentDialogResult.Secondary => FeedbackResult.NeverShowAgain,
            _ => FeedbackResult.Cancel
        };
        if (result.Result == FeedbackResult.Submit)
        {
            result.Stars = rating.Value;
            result.Content = content.Text;
            //result.Account = accountHandler.Current.Id;
        }

        if (result.Result == FeedbackResult.NeverShowAgain)
            configuration.Launcher.AskForFeedback = false;

        logger.Log($"Showed feedback request [{result.Result}]");
        return result;
    }


    /// <summary>
    /// Submits a given feedback to the connected API
    /// </summary>
    /// <param name="feedback">The feedback which should be submitted</param>
    /// <returns>The boolean wether the action was successful</returns>
    public async Task<bool> SubmitAsync(
        Feedback feedback)
    {
        try
        {
            // API STUFF => API NOT DONE: LOG FEEDBACK
            await Task.Delay(1000);

            logger.Log($"Submitted feddback [{feedback.Stars}-{feedback.Content}]");
            return true;
        }
        catch (Exception ex)
        {
            logger.Log($"Failed to submit feddback [{feedback.Stars}-{feedback.Content}]", ex);
            return false;
        }
    }
    #endregion
}