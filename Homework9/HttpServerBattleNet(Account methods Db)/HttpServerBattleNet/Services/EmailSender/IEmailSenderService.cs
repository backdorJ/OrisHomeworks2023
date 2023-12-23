namespace HttpServerBattleNet.Services.EmailSender;

public interface IEmailSenderService
{
    void SendEmail(string emailFromUser, string passwordFromUser, string subject);
}