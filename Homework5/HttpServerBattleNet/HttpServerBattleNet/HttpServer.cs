using System.Net;
namespace HttpServerBattleNet;

public class HttpServer
{
    private HttpListener _listener;
    private ServerHandler _serverHandler;
    private bool _running = false;
    private CancellationTokenSource cts = new();
    private static readonly Lazy<HttpServer> _lazy = new Lazy<HttpServer>(() => new HttpServer());

    private HttpServer()
    {
        _listener = new HttpListener();
        _serverHandler = new ServerHandler();
    }

    public static HttpServer Instance => _lazy.Value;

    public async Task StartAsync()
    {
        _running = true;
        await Task.Run(async () => { await Run(); });
    }

    private async Task Run()
    {
        var config = await _serverHandler.GetConnectionConfigurationServer("appsettings.json");
        _listener.Prefixes.Add($"{config.Address}:{config.Port}/");
        Console.WriteLine($"Server has been started. For address: {config.Address}:{config.Port}");
        var token = cts.Token;
        _listener.Start();
        
        Task.Run(ProcessCallback);

        while (_running)
        {
            if (token.IsCancellationRequested)
                return;
            
            var context = await _listener.GetContextAsync();
            var request = context.Request;
            using var response = context.Response;
            var requestUrl = request.Url!.AbsolutePath;
            var pathOfSource = Path.Combine(config.StaticPathFiles, requestUrl.Trim('/'));
            
            if (requestUrl == "/login")
            {
                var dataOfUser = _serverHandler.GetCurrentUserData(request);
                Console.WriteLine("Message has been sent on email.");
            }
            else if (requestUrl.EndsWith('/'))
            {
                var pathOfIndex = Path.Combine(config.StaticPathFiles, "index.html");
                if (File.Exists(pathOfIndex)) await _serverHandler.InvokePageAsync(response, pathOfIndex, token);
                else await _serverHandler.InvokeFailedPageAsync(response, pathOfIndex, token);
            }
            else
            {
                if (File.Exists(pathOfSource)) await _serverHandler.InvokePageAsync(response, pathOfSource, token);
                else await _serverHandler.InvokeFailedPageAsync(response, pathOfSource, token);
            }
        }

        _running = false;
        _listener.Close();
        ((IDisposable)_listener).Dispose();
        Console.WriteLine("Server has been stopped.");
    }
    

    private void ProcessCallback()
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