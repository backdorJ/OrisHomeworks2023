namespace HttpServerBattleNet.Data.MyORM;

public interface IDatabaseOperation
{
    bool Add<T>(T entity);
    bool Update<T>(T entity);
    bool Delete<T>(int id);
    List<T> Select<T>(T entity);
    T SelectById<T>(int id);
}