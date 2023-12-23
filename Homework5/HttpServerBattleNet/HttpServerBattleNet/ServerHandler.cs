using System.Net;
using System.Text;
using HttpServerBattleNet.Configuration;
using Newtonsoft.Json;

namespace HttpServerBattleNet;

public class ServerHandler
{
    internal async Task<AppSettingsConfig> GetConnectionConfigurationServer(string fileName)
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

    internal async Task InvokePageAsync(HttpListenerResponse response, string filePath, CancellationToken token)
    {
        var contentType = GetContentType(filePath[filePath.IndexOf('.')..]);
        response.ContentType = contentType;
        var buffer = await File.ReadAllBytesAsync(filePath, token);
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, token);
    }

    internal async Task InvokeFailedPageAsync(HttpListenerResponse response, string filePath, CancellationToken token)
    {
        response.StatusCode = (int)HttpStatusCode.NotFound;
        response.ContentType = "text/plain; charset=utf-8";
        var buffer = Encoding.UTF8.GetBytes("404 File Not Found - файл не найден");
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, token);
        Console.WriteLine("File not founded");
    }

    internal string[] GetCurrentUserData(HttpListenerRequest request)
    {
        using var streamReader = new StreamReader(request.InputStream);
        var tempOfData = streamReader.ReadToEnd();
        var currentOfUserData = tempOfData?.Split('&');
        return new string[] { WebUtility.UrlDecode(currentOfUserData[0][6..]), currentOfUserData[1][9..] };
    }

    private string GetContentType(string type)
    {
        return type switch
        {
            ".html" => "text/html",
            ".css" => "text/css",
            ".jpg" => "image/jpeg",
            ".svg" => "image/svg+xml",
            ".png" => "image/png",
            _ => ""
        };
    }

    private void EnsureStaticFilePath(AppSettingsConfig config)
    {
        try
        {
            if (!Directory.Exists(config.StaticPathFiles))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), config.StaticPathFiles));
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