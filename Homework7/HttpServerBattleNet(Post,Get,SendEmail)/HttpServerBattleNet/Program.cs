using HttpServerBattleNet;

class Program
{
    private static async Task Main()
    {
        var server = new HttpServer();
        await server.StartAsync();
    }
}