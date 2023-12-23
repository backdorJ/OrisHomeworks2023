using HttpServerBattleNet.Attribuets;
using HttpServerBattleNet.Services.EmailSender;

namespace HttpServerBattleNet.Controllers;

[Controller("Account")]
public class AccountController
{
    [Post("SendToEmail")]
    public void SendToEmail(string emailFromUser, string passwordFromUser)
    {
        new EmailSenderService().SendEmail(emailFromUser, passwordFromUser, "");
        Console.WriteLine("Email has been sent.");
    }
}