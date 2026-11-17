using Android.App;
using Android.Runtime;
using Microsoft.Maui;

namespace Editor3D;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
