using HttpServerBattleNet.Data.Interfaces;
using HttpServerBattleNet.Data.MyORM;
using HttpServerBattleNet.Model;

namespace HttpServerBattleNet.Data.Repositories;

public class AccountRepository
    : IAccountRepository
{
    private readonly MyDataContext _context
        = new MyDataContext(Connection.ConnectionString);

    public bool Add(Account entity)
        => _context.Add(entity);

    public bool Update(Account entity)
        => _context.Update(entity);

    public bool Delete(int id)
        => _context.Delete<Account>(id);

    public List<Account> Select(Account entity)
        => _context.Select(entity);

    public Account SelectById(int id)
        => _context.SelectById<Account>(id);
}