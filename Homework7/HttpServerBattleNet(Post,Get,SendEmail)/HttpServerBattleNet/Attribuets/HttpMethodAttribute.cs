namespace HttpServerBattleNet.Attribuets;

public class HttpMethodAttribute : Attribute
{
    public HttpMethodAttribute(string actionName)
    {
        ActionName = actionName;
    }

    public string ActionName { get; set; }
}