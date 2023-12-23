namespace HttpServerBattleNet.Attribuets;

public class PostAttribute : HttpMethodAttribute
{
    public PostAttribute(string actionName) : base(actionName)
    {
    }
}