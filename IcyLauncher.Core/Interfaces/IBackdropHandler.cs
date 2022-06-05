using Microsoft.UI.Composition.SystemBackdrops;

namespace IcyLauncher.Core.Interfaces;

public interface IBackdropHandler
{
    Window Shell { get; }
    object Controller { get; }
    SystemBackdropConfiguration BackdropConfiguration { get; }
    WindowsSystemDispatcherQueueHelper DispatcherQueueHelper { get; }

    bool SetBackdrop();
}