using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Game.utils.Paths;

namespace Server;

internal class ServerObject
{
    private readonly TcpListener _tcpListener = new(IPAddress.Any, 8888);
    private readonly List<ClientObject> _clients = new();
    private readonly List<SendPoint> _pointsField = new();

    protected internal void RemoveConnection(string id)
    {
        var client = _clients.FirstOrDefault(c => c.Id.Equals(id));

        if (client != null)
            _clients.Remove(client);
        client?.Close();
    }

    protected internal void Listen()
    {
        try
        {
            _tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                var tcpClient = _tcpListener.AcceptTcpClient();

                var clientObject = new ClientObject(tcpClient, this);
                _clients.Add(clientObject);
                Task.Run(clientObject.ProcessAsync);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    private string GenerateRandomColor()
    {
        var random = new Random();
        var color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        var hexColor = ColorTranslator.ToHtml(color);
        while (_clients.Select(i => i.Color).Contains(hexColor))
            color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        return ColorTranslator.ToHtml(color);
    }

    protected internal async Task SendListAsync()
    {
        var sb = new StringBuilder();
        sb.Append("SendList ");
        sb.Append(JsonSerializer.Serialize(_clients.Select(x => new AddUser(x.UserName!, x.Color!)).ToList()));

        foreach (var client in _clients)
        {
            await client.Writer.WriteLineAsync(sb);
            await client.Writer.FlushAsync();
        }
    }

    protected internal async Task BroadcastColoredMessageAsync(AddUser addUser)
    {
        var color = GenerateRandomColor();
        addUser.Color = color;
        _clients.Last().Color = color;

        var sb = new StringBuilder();
        sb.Append("AddUser ");
        var message = JsonSerializer.Serialize(addUser);
        sb.Append(message);
        {
            try
            {
                await _clients.Last().Writer.WriteLineAsync(sb);
                await _clients.Last().Writer.FlushAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при отправке сообщения клиенту: " + ex.Message);
            }
        }
    }

    protected internal async Task BroadcastPointsFieldMessageAsync()
    {
        foreach (var point in _pointsField)
        {
            var sb = new StringBuilder();
            sb.Append("SendPoint ");
            {
                try
                {
                    var message = JsonSerializer.Serialize(point);
                    sb.Append(message);
                    await _clients.Last().Writer.WriteLineAsync(sb);
                    await _clients.Last().Writer.FlushAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при отправке точки клиенту: " + ex.Message);
                }
            }
        }
    }

    protected internal async Task BroadcastPointMessageAsync(SendPoint point)
    {
        var sb = new StringBuilder();
        sb.Append("SendPoint ");
        {
            try
            {
                sb.Append(JsonSerializer.Serialize(point));
                foreach (var client in _clients)
                {
                    await client.Writer.WriteLineAsync(sb);
                    await client.Writer.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при отправке точки клиенту: " + ex.Message);
            }
        }
    }

    protected internal void Disconnect()
    {
        foreach (var client in _clients)
            client.Close();
        _tcpListener.Stop();
    }

    protected internal async Task BroadcastMessageAsync(string message, string id)
    {
        var usersToJson = JsonSerializer.Serialize(_clients.Select(x => x.UserName).ToList());
        foreach (var client in _clients)
        {
            await client.Writer.WriteLineAsync(usersToJson); //передача данных
            await client.Writer.FlushAsync();
        }
    }

    protected internal async Task AddPoint(SendPoint point)
    {
        _pointsField.Add(point);
        await BroadcastPointMessageAsync(point);
    }
}