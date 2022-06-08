using Microsoft.UI.Composition.SystemBackdrops;

namespace IcyLauncher.Core.Interfaces;

public interface IBackdropHandler
{
    Window Shell { get; }
    object Controller { get; }
    SystemBackdropConfiguration BackdropConfiguration { get; }
    WindowsSystemDispatcherQueueHelper DispatcherQueueHelper { get; }

    bool SetBackdrop(bool useDarkMode);
}


// => Disabled correct way to implement Mica/Acrylic because of limitations
// => Placed in servises registration
//switch (configuration.Apperance.Blur)
//{
//    case BlurEffect.Mica:
//        services.AddScoped<IBackdropHandler, MicaBackdropHandler>();
//        break;
//    case BlurEffect.Acrylic:
//        services.AddScoped<IBackdropHandler, AcrylicBackdropHandler>();
//        break;
//}

// => Disabled correct way to implement Mica/Acrylic because of limitations
// => Placed in AppStartupHandler
//var backdropHandler = Provider.GetService<IBackdropHandler>();
//if (backdropHandler is not null) 
//    backdropHandler.SetBackdrop();