using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MyHttpServer;

public class HttpServer
{
    private HttpListener _listener;
    private bool _running = false;
    private CancellationTokenSource cts = new();
    private object _lock = new();

    public HttpServer()
    {
        _listener = new HttpListener();
    }

    public async Task Start()
    {
        await Task.Run(async () =>
        {
            await Run();
        });
    }

    private async Task Run()
    {
        _running = true;
        var config = await GetConnectionConfigurationServer("appsettings.json");
        _listener.Prefixes.Add($"{config.Address}:{config.Port}/");
        Console.WriteLine($"Server has been started. For address: {config.Address}:{config.Port}");
        var token = cts.Token;
        _listener.Start();

        Task.Run(ProcessCallback);

        while (_running)
        {
            var context = await _listener.GetContextAsync();
            Task.Run(async () =>
            {
                var request = context.Request;
                using var response = context.Response;
                var requestUrl = request.Url.AbsolutePath;
                if (requestUrl.EndsWith(".html"))
                {
                    var filePath = Path.Combine("." + requestUrl);
                    if (File.Exists(filePath))
                        await DisplayFoundPageAsync(response, filePath, token);
                    else
                        DisplayNotFoundPage(response);
                }
                else
                {
                    var mainPagePath = Path.Combine(config.StaticPathFiles, "index.html");
                    if (File.Exists(mainPagePath))
                        await DisplayFoundPageAsync(response, mainPagePath, token);
                    else
                        DisplayNotFoundPage(response);
                }
                response.Close();
            });
        }

        _running = false;
        _listener.Close();
        ((IDisposable)_listener).Dispose();
        Console.WriteLine("Server has been stopped.");
    }

    private void DisplayNotFoundPage(HttpListenerResponse response)
    {
        response.StatusCode = (int)HttpStatusCode.NotFound;
        response.ContentType = "text/plain; charset=utf-8";
        var buffer = Encoding.UTF8.GetBytes("404 File Not Found - файл не найден");
        response.ContentLength64 = buffer.Length;
        response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        Console.WriteLine("File not founded");
    }

    private async Task DisplayFoundPageAsync(HttpListenerResponse response, string pathPage, CancellationToken token)
    {
        response.ContentType = "text/html;";
        var buffer = await File.ReadAllBytesAsync(pathPage, token);
        response.ContentLength64 = buffer.Length;
        response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }
    
    private void ProcessCallback()
    {
        lock (_lock)
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "stop")
                {
                    _running = false;
                    cts.Cancel();
                    break;
                }
            }
        }
    }

    private async Task<AppSettingsConfig> GetConnectionConfigurationServer(string fileName)
    {
        try
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл appsettings.json не найден");
                throw new FileNotFoundException();
            }

            var json = await File.OpenText(fileName).ReadToEndAsync();
            var obj = JsonConvert.DeserializeObject<AppSettingsConfig>(json);
            EnsureStaticFilePath(obj);
            return obj;
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка при десериализации");
            throw;
        }
    }

    private void EnsureStaticFilePath(AppSettingsConfig config)
    {
        try
        {
            if (!Directory.Exists(config.StaticPathFiles))
            {
                Directory.CreateDirectory(config.StaticPathFiles);
                Console.WriteLine("Была создана папка static в пути {configPath}", config.StaticPathFiles);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Не удалось создать папку по указаному пути: {config.StaticPathFiles}");
            throw;
        }
    }
}