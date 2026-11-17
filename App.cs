using Microsoft.Maui.Controls;

namespace Editor3D;

public sealed class App : Application
{
    public App()
    {
        try
        {
            MainPage = new EditorPage();
        }
        catch (Exception ex)
        {
#if ANDROID
            Android.Util.Log.Error("EDITOR3D_STARTUP", ex.ToString());
#endif
            MainPage = BuildDiagnosticPage(ex);
        }
    }

    private static ContentPage BuildDiagnosticPage(Exception ex)
    {
        var details = new Label
        {
            Text = ex.ToString(),
            TextColor = Colors.White,
            FontSize = 13
        };
        var close = new Button
        {
            Text = "Reintentar abrir editor",
            BackgroundColor = Color.FromArgb("#3567F6"),
            TextColor = Colors.White
        };
        close.Clicked += (_, _) =>
        {
            try { Current!.MainPage = new EditorPage(); }
            catch (Exception retry) { Current!.MainPage = BuildDiagnosticPage(retry); }
        };
        return new ContentPage
        {
            Title = "Error de inicio",
            BackgroundColor = Color.FromArgb("#17191F"),
            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 24,
                    Spacing = 14,
                    Children =
                    {
                        new Label { Text = "El editor no pudo iniciar", FontSize = 22, TextColor = Colors.White, FontAttributes = FontAttributes.Bold },
                        new Label { Text = "Copia este error para identificar la causa:", TextColor = Colors.LightGray },
                        details,
                        close
                    }
                }
            }
        };
    }
}
