namespace HttpServerBattleNet.Attribuets;

public class ControllerAttribute : Attribute
{
    public ControllerAttribute(string controllerName)
    {
        ControllerName = controllerName;
    }

    public string ControllerName { get; }
}