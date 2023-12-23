using System.Net;
using HttpServerBattleNet.asda;
using HttpServerBattleNet.Configuration;
using HttpServerBattleNet.Services;

namespace HttpServerBattleNet.Handler;
 
public class StaticFileHandlers : Handler
{
    private AppSettingsConfig _config = ServerConfiguration.Config;
        
    public override void HandleRequest(HttpListenerContext context)
    {
        // некоторая обработка запроса
        var request = context.Request;
        using var response = context.Response;
        var absoluteRequestUrl = request.Url!.AbsolutePath;
        var pathOfStaticFile = Path.Combine(_config.StaticPathFiles, absoluteRequestUrl.Trim('/'));

        
        if (absoluteRequestUrl!.Split('/')!.LastOrDefault()!.Contains('.'))
        {
            var pattern = absoluteRequestUrl?.Split('/')?.LastOrDefault();
            pattern = pattern?[pattern.IndexOf('.')..];
            if (File.Exists(pathOfStaticFile) && pattern != null)
            {
                response.ContentType = DictionaryExtensions._dictOfExtenshions[pattern];
                using var fileStream = File.OpenRead(pathOfStaticFile);
                fileStream.CopyTo(response.OutputStream);   
            }
            else
            {
                using var fileStream = File.OpenRead(Path.Combine(_config.StaticPathFiles, "404.html"));
                fileStream.CopyTo(response.OutputStream);
            }
        }
        // передача запроса дальше по цепи при наличии в ней обработчиков
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}