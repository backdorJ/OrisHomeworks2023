using HttpServerBattleNet.Attribuets;
using HttpServerBattleNet.Data.Interfaces;
using HttpServerBattleNet.Data.Repositories;
using HttpServerBattleNet.Model;
using HttpServerBattleNet.Services.EmailSender;

namespace HttpServerBattleNet.Controllers;

[Controller("Account")]
public class AccountController
{
    private readonly IAccountRepository _accountRepository = new AccountRepository();
    
    [Post("SendToEmail")]
    public void SendToEmail(string emailFromUser, string passwordFromUser)
    {
        new EmailSenderService().SendEmail(emailFromUser, passwordFromUser, "");
        Console.WriteLine("Email has been sent.");
    }

    [Post("Add")]
    public void Add(string login, string password)
        => _accountRepository.Add(
            new Account
            {
                Login = login,
                Password = password
            });

    [Get("GetAll")]
    public Account[] GetAll()
        => _accountRepository
            .Select(new Account())
            .ToArray();

    [Get("GetById")]
    public Account GetById(string id)
        => _accountRepository.SelectById(int.Parse(id));

    [Post("Delete")]
    public string Delete(string id)
    {
        _accountRepository.Delete(int.Parse(id));

        return "Account has been removed.";
    }

    [Post("Update")]
    public Account Update(string login, string password, string id)
    {
        var entity = _accountRepository.SelectById(int.Parse(id));

        if (entity is null)
            return new Account() { Login = "Not Found" };

        entity.Password = password;
        entity.Login = login;
        
        _accountRepository.Update(entity);
        return entity;
    }
}