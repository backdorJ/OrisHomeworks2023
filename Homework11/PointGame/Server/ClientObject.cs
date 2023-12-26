using System.Net.Sockets;
using System.Text.Json;
using Game.utils.Paths;

namespace Server;

internal class ClientObject
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal StreamWriter Writer { get; }
    protected internal StreamReader Reader { get; }
    public string? UserName { get; set; }
    public string? Color { get; set; }

    private readonly TcpClient _client;
    private readonly ServerObject _server;

    public ClientObject(TcpClient tcpClient, ServerObject serverObject)
    {
        _client = tcpClient;
        _server = serverObject;

        var stream = _client.GetStream();

        Reader = new StreamReader(stream);

        Writer = new StreamWriter(stream);
    }


    public async Task ProcessAsync()
    {
        try
        {
            UserName = await Reader.ReadLineAsync();
            var addUserMessage = new AddUser { UserName = UserName, Color = ""};
            await _server.BroadcastColoredMessageAsync(addUserMessage);

            var message = $"{UserName} вошел в чат";
            Console.WriteLine(message);

            await _server.SendListAsync();
            await _server.BroadcastPointsFieldMessageAsync();

            while (true)
            {
                await Task.Delay(10);

                try
                {
                    message = await Reader.ReadLineAsync();
                    var point = JsonSerializer.Deserialize<SendPoint>(message!);
                    await _server.AddPoint(point!);
                }
                catch
                {
                    message = $"{UserName} покинул игру";
                    Console.WriteLine(message);
                    _server.RemoveConnection(Id);
                    await _server.SendListAsync();
                    await _server.BroadcastMessageAsync(message, Id);
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            _server.RemoveConnection(Id);
        }
    }

    protected internal void Close()
    {
        Writer.Close();
        Reader.Close();
        _client.Close();
    }
}