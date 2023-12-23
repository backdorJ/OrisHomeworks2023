namespace HttpServerBattleNet.Attribuets;

public class GetAttribute : HttpMethodAttribute
{
    public GetAttribute(string actionName) : base(actionName)
    {
    }
}