using System.Net;
using System.Text;
using MyHttpServer;
using Newtonsoft.Json;
using HttpListener = System.Net.HttpListener;

class Program
{
    static async Task Main()
    {
        var server = new HttpServer();
        await server.Start();
    }   
}