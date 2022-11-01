using IcyLauncher.Data.Types;

namespace IcyLauncher.Xaml.UI;

public sealed partial class FeedbackDialog : ContentDialog
{
    public static readonly DependencyProperty RatingProperty = DependencyProperty.Register(
        "Rating", typeof(int), typeof(FeedbackDialog), new(-1));

    public int Rating
    {
        get => (int)GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }

    public static readonly DependencyProperty FeedbackTextProperty = DependencyProperty.Register(
        "FeedbackText", typeof(string), typeof(FeedbackDialog), new(string.Empty));

    public string FeedbackText
    {
        get => (string)GetValue(FeedbackTextProperty);
        set => SetValue(FeedbackTextProperty, value);
    }

    public FeedbackResult Result { get; private set; }


    public FeedbackDialog(
        bool forceShow = false)
    {
        InitializeComponent();

        SecondaryButtonText = forceShow ? null : "Never show again :(";

        Opened += (s, e) =>
        {
            Result = FeedbackResult.Cancel;
        };
        Closing += (s, e) =>
        {
            Result = e.Result switch
            {
                ContentDialogResult.Primary => FeedbackResult.Submit,
                ContentDialogResult.Secondary => FeedbackResult.NeverShowAgain,
                _ => FeedbackResult.Cancel
            };
        };

        RatingControl.ValueChanged += (s, e) =>
        {
            FeedbackTextControl.Visibility = RatingControl.Value == -1 ? Visibility.Collapsed : Visibility.Visible;
            IsPrimaryButtonEnabled = RatingControl.Value != -1;
        };
    }
}