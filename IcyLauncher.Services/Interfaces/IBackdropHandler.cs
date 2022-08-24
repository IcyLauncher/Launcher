namespace IcyLauncher.Services.Interfaces;

public interface IBackdropHandler
{
    bool SetBackdrop(bool useDarkMode);
}

// => Disabled correct way to implement Mica/Acrylic because of limitations => Need to activate on WndowsAppSDK v1.1.0-stable
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