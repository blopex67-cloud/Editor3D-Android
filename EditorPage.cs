using System.Text.Json;

namespace Editor3D;

public sealed class EditorPage : ContentPage
{
    private readonly WebView _webView;

    public EditorPage()
    {
        Title = "Editor 3D";
        BackgroundColor = Color.FromArgb("#111318");
        _webView = new WebView
        {
            Source = new UrlWebViewSource { Url = "file:///android_asset/wwwroot/index.html" },
            BackgroundColor = Color.FromArgb("#111318")
        };
        _webView.Navigating += OnNavigating;
        Content = _webView;
    }

    private async void OnNavigating(object? sender, WebNavigatingEventArgs e)
    {
        if (!Uri.TryCreate(e.Url, UriKind.Absolute, out var uri) || uri.Scheme != "zapia") return;
        e.Cancel = true;
        try
        {
            if (uri.Host == "save")
            {
                var json = Uri.UnescapeDataString(uri.Query.TrimStart('?'));
                var file = Path.Combine(FileSystem.AppDataDirectory, "escena-editor3d.json");
                await File.WriteAllTextAsync(file, json);
                await DisplayAlert("Escena guardada", $"Guardada en el almacenamiento de la app.\n{file}", "OK");
            }
            else if (uri.Host == "import")
            {
                var picked = await FilePicker.Default.PickAsync(new PickOptions { PickerTitle = "Importar escena JSON" });
                if (picked is null) return;
                var json = await File.ReadAllTextAsync(picked.FullPath);
                var arg = JsonSerializer.Serialize(json);
                await _webView.EvaluateJavaScriptAsync($"window.loadSceneFromNative({arg})");
            }
            else if (uri.Host == "export")
            {
                var json = Uri.UnescapeDataString(uri.Query.TrimStart('?'));
                var file = Path.Combine(FileSystem.CacheDirectory, "escena-editor3d.json");
                await File.WriteAllTextAsync(file, json);
                await Share.Default.RequestAsync(new ShareFileRequest("Compartir escena 3D", new ShareFile(file)));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("No se pudo completar", ex.Message, "OK");
        }
    }
}
