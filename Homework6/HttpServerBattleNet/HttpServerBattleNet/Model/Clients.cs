namespace HttpServerBattleNet.Model;

public class Clients
{
    public int Id { get; set; }
    public int Status { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsBlocked { get; set; }
    public bool IsAnonymous { get; set; }
}