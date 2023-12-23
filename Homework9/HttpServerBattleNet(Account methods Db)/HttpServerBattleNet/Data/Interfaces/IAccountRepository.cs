using HttpServerBattleNet.Model;

namespace HttpServerBattleNet.Data.Interfaces;

public interface IAccountRepository
{
    bool Add(Account entity);
    bool Update(Account entity);
    bool Delete(int id);
    List<Account> Select(Account entity);
    Account SelectById(int id);
}