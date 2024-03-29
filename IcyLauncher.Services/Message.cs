﻿using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace IcyLauncher.Services;

public class Message : IMessage
{
    #region Setup
    readonly ILogger logger;
    readonly CoreWindow shell;

    /// <summary>
    /// Service to queue dialogs and notifications on the current main window
    /// </summary>
    public Message(
        ILogger<Message> logger,
        CoreWindow shell)
    {
        this.logger = logger;
        this.shell = shell;

        logger.Log("Registered message");
    }
    #endregion


    #region Active Dialog
    ContentDialog? activeDialog = null;
    TaskCompletionSource<bool> dialogAwaiter = new();

    void OnActiveDialogClosed(ContentDialog _, ContentDialogClosedEventArgs _1)
    {
        dialogAwaiter.TrySetResult(true);

        if (activeDialog is not null)
            activeDialog.Closed -= OnActiveDialogClosed;
    }
    #endregion


    #region Actions Async
    /// <summary>
    /// Queues a given dialog asynchronously and optionally waits until the previous dialog is closed
    /// </summary>
    /// <param name="dialog">The dialog which should be shown</param>
    /// <param name="awaitPreviousDialog">Whether it should wait until the previous dialog is closed</param>
    /// <returns>The result of the dialog (ContentDialogResult)</returns>
    public async Task<ContentDialogResult> ShowAsync(
        ContentDialog dialog,
        bool awaitPreviousDialog = false)
    {
        logger.Log($"Requested dialog [await:{awaitPreviousDialog}]");

        if (activeDialog is not null)
        {
            if (awaitPreviousDialog)
            {
                await dialogAwaiter.Task;

                dialogAwaiter = new();
            }

            activeDialog.Hide();
        }

        dialog.XamlRoot = shell.Content.XamlRoot;
        dialog.RequestedTheme = ElementTheme.Dark;
        activeDialog = dialog;

        activeDialog.Closed += OnActiveDialogClosed;
        return await activeDialog.ShowAsync();
    }

    /// <summary>
    /// Queues a new dialog asynchronously and optionally waits until the previous dialog is closed
    /// </summary>
    /// <param name="title">The title of the dialog</param>
    /// <param name="content">The content of the dialog</param>
    /// <param name="awaitPreviousDialog">Whether it should wait until the previous dialog is closed</param>
    /// <param name="closeButton">The content of the close button</param>
    /// <param name="primaryButton">The content of the primary button</param>
    /// <param name="secondaryButton">The content of the secondary button</param>
    /// <returns>The result of the dialog (ContentDialogResult)</returns>
    public async Task<ContentDialogResult> ShowAsync(
        string title,
        object content,
        bool awaitPreviousDialog = false,
        string? closeButton = "Ok",
        string? primaryButton = null,
        string? secondaryButton = null) =>
        await ShowAsync(
            new ContentDialog()
            {
                RequestedTheme = ElementTheme.Dark,
                Title = title,
                Content = content,
                CloseButtonText = closeButton,
                PrimaryButtonText = primaryButton,
                SecondaryButtonText = secondaryButton
            }, awaitPreviousDialog);
    #endregion

    #region Actions
    /// <summary>
    /// Queues a new dialog
    /// </summary>
    /// <param name="title">The title of the dialog</param>
    /// <param name="content">The content of the dialog</param>
    /// <param name="closeButton">The content of the close button</param>
    /// <param name="primaryButton">The content of the primary button</param>
    /// <param name="secondaryButton">The content of the secondary button</param>
    public async void Show(
        string title,
        object content,
        string? closeButton = "Ok",
        string? primaryButton = null,
        string? secondaryButton = null) =>
        await ShowAsync(title, content, false, closeButton, primaryButton, secondaryButton);
    #endregion
}