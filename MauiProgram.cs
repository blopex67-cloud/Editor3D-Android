using Microsoft.Maui.Handlers;

namespace Editor3D;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();
#if ANDROID
        WebViewHandler.Mapper.AppendToMapping("EditorWebView", (handler, view) =>
        {
            handler.PlatformView.Settings.JavaScriptEnabled = true;
            handler.PlatformView.Settings.DomStorageEnabled = true;
            handler.PlatformView.Settings.AllowFileAccess = true;
            handler.PlatformView.Settings.AllowFileAccessFromFileURLs = true;
            handler.PlatformView.Settings.AllowUniversalAccessFromFileURLs = true;
            handler.PlatformView.Settings.AllowContentAccess = true;
        });
#endif
        return builder.Build();
    }
}
