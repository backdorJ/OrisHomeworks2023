using System.Net;

namespace HttpServerBattleNet.Handler;

public abstract class Handler
{
    public Handler Successor { get; set; }
    public abstract void HandleRequest(HttpListenerContext context);
}