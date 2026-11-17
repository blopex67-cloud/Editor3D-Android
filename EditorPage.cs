using Microsoft.Maui.Controls;
using System.Text;
using System.Text.Json;

namespace Editor3D;

public sealed class EditorPage : ContentPage
{
    private readonly HybridWebView _webView;

    public EditorPage()
    {
        Title = "Editor 3D";
        BackgroundColor = Color.FromArgb("#111318");
        _webView = new HybridWebView
        {
            HybridRoot = "wwwroot",
            DefaultFile = "index.html",
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };
        _webView.RawMessageReceived += OnRawMessageReceived;
        Content = _webView;
    }

    private async void OnRawMessageReceived(object? sender, HybridWebViewRawMessageReceivedEventArgs e)
    {
        try
        {
            var separator = e.Message.IndexOf('|');
            if (separator < 0) return;
            var action = e.Message[..separator];
            var payload = e.Message[(separator + 1)..];

            switch (action)
            {
                case "save":
                {
                    using var document = JsonDocument.Parse(payload);
                    var file = Path.Combine(FileSystem.AppDataDirectory, "escena-editor3d.json");
                    await File.WriteAllTextAsync(file, payload, Encoding.UTF8);
                    await DisplayAlert("Escena guardada", "Quedó guardada en este dispositivo.", "OK");
                    break;
                }
                case "import":
                {
                    var picked = await FilePicker.Default.PickAsync(new PickOptions
                    {
                        PickerTitle = "Elige una escena JSON"
                    });
                    if (picked is null) return;
                    await using var stream = await picked.OpenReadAsync();
                    using var reader = new StreamReader(stream, Encoding.UTF8);
                    var json = await reader.ReadToEndAsync();
                    using var parsed = JsonDocument.Parse(json);
                    if (!parsed.RootElement.TryGetProperty("objects", out var objects) || objects.ValueKind != JsonValueKind.Array)
                        throw new InvalidDataException("El archivo no contiene una escena reconocible.");
                    _webView.SendRawMessage("load|" + json);
                    break;
                }
                case "export":
                {
                    using var document = JsonDocument.Parse(payload);
                    var file = Path.Combine(FileSystem.CacheDirectory, "escena-editor3d.json");
                    await File.WriteAllTextAsync(file, payload, Encoding.UTF8);
                    await Share.Default.RequestAsync(new ShareFileRequest("Compartir escena 3D", new ShareFile(file)));
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("No se pudo completar", ex.Message, "OK");
        }
    }
}
