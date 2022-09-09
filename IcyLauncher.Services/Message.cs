using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace IcyLauncher.Services;

public class Message : IMessage
{
    readonly ILogger logger;
    readonly UIElementReciever uiElementReciever;

    public Message(
        ILogger<Message> logger,
        UIElementReciever uiElementReciever)
    {
        this.logger = logger;
        this.uiElementReciever = uiElementReciever;

        logger.Log("Registered Message serivce and initialized content dialog");
    }


    ContentDialog? activeDialog = null;
    TaskCompletionSource<bool> dialogAwaiter = new();

    void OnActiveDialogClosed(ContentDialog _, ContentDialogClosedEventArgs _1)
    {
        dialogAwaiter.TrySetResult(true);
        if (activeDialog is not null)
            activeDialog.Closed -= OnActiveDialogClosed;
    }


    public async Task<ContentDialogResult> ShowAsync(
        string title,
        object content,
        bool awaitPreviousDialog,
        string? closeButton = "Cancel",
        string? primaryButton = null,
        string? secondaryButton = null)
    {
        logger.Log($"Requested dialog: AwaitPrev-{awaitPreviousDialog}");

        if (activeDialog is not null)
        {
            if (awaitPreviousDialog)
            {
                await dialogAwaiter.Task;
                dialogAwaiter = new();
            }
            else
                activeDialog.Hide();
        }

        activeDialog = new ContentDialog()
        {
            XamlRoot = uiElementReciever.MainGrid.XamlRoot,
            RequestedTheme = ElementTheme.Dark,
            Title = title,
            Content = content,
            CloseButtonText = closeButton,
            PrimaryButtonText = primaryButton,
            SecondaryButtonText = secondaryButton
        };

        activeDialog.Closed += OnActiveDialogClosed;
        return await activeDialog.ShowAsync();
    }

    public async void Show(
        string title,
        object content,
        bool awaitPreviousDialog,
        string? closeButton = "Cancel",
        string? primaryButton = null,
        string? secondaryButton = null) =>
        await ShowAsync(title, content, awaitPreviousDialog, closeButton, primaryButton, secondaryButton);

}