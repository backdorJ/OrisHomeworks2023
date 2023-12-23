namespace HttpServerBattleNet.Services;

public interface IEmailSenderService
{
    void SendEmail(string emailFromUser, string passwordFromUser, string subject);
}