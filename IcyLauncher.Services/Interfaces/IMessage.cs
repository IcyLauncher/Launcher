using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace IcyLauncher.Services.Interfaces;

public interface IMessage
{
    Task<ContentDialogResult> ShowAsync(string title, object content, bool awaitPreviousDialog, string? closeButton = "Cancel", string? primaryButton = null, string? secondaryButton = null);

    void Show(string title, object content, bool awaitPreviousDialog, string? closeButton = "Cancel", string? primaryButton = null, string? secondaryButton = null);
}