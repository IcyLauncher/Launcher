﻿using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace IcyLauncher.Services.Interfaces;

public interface IMessage
{
    #region Actions Async
    /// <summary>
    /// Queues a given dialog asynchronously and optionally waits until the previous dialog is closed
    /// </summary>
    /// <param name="dialog">The dialog which should be shown</param>
    /// <param name="awaitPreviousDialog">Whether it should wait until the previous dialog is closed</param>
    /// <returns>The result of the dialog (ContentDialogResult)</returns>
    Task<ContentDialogResult> ShowAsync(
        ContentDialog dialog,
        bool awaitPreviousDialog = false);

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
    Task<ContentDialogResult> ShowAsync(
        string title,
        object content,
        bool awaitPreviousDialog = false,
        string? closeButton = "Ok",
        string? primaryButton = null,
        string? secondaryButton = null);
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
    void Show(
        string title,
        object content,
        string? closeButton = "Ok",
        string? primaryButton = null,
        string? secondaryButton = null);
    #endregion
}