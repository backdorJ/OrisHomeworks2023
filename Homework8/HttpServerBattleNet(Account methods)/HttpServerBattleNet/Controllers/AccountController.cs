using HttpServerBattleNet.Attribuets;
using HttpServerBattleNet.Model;
using HttpServerBattleNet.Services.EmailSender;

namespace HttpServerBattleNet.Controllers;

[Controller("Account")]
public class AccountController
{
    private static ICollection<Account> _accounts
        = new List<Account>()
        {
            new()
            {
                Id = 1,
                Login = "Damir",
                Password = "123"
            },
            new()
            {
                Id = 2,
                Login = "Liza",
                Password = "1234"
            },
        };
    
    [Post("SendToEmail")]
    public void SendToEmail(string emailFromUser, string passwordFromUser)
    {
        new EmailSenderService().SendEmail(emailFromUser, passwordFromUser, "");
        Console.WriteLine("Email has been sent.");
    }

    [Post("Add")]
    public void Add(string login, string password)
        => _accounts.Add(
            new Account
            {
                Id = _accounts.Max(x => x.Id) + 1,
                Login = login,
                Password = password
            });

    [Get("GetAll")]
    public Account[] GetAll()
        => _accounts.ToArray();

    [Get("GetById")]
    public Account GetById(string id)
        => _accounts.FirstOrDefault(x => x.Id == int.Parse(id))
           ?? new Account() { Login = "Not Found" };

    [Post("Delete")]
    public string Delete(string id)
    {
        var entity = _accounts.FirstOrDefault(x => x.Id == int.Parse(id));

        if (entity is null)
            return "Not found";

        _accounts.Remove(entity);

        return "Account has been removed.";
    }

    [Post("Update")]
    public Account Update(string login, string password, string id)
    {
        var entity = _accounts.FirstOrDefault(x => x.Id == int.Parse(id));

        if (entity is null)
            return new Account() { Login = "Not Found" };

        _accounts.Remove(entity);

        entity.Password = password;
        entity.Login = login;

        _accounts.Add(entity);

        return entity;
    }
}