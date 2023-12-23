using HttpServerBattleNet;

class Program
{
    private static async Task Main()
    {
        var server = HttpServer.Instance;
        await server.StartAsync();
        Console.ReadKey();
    }
}