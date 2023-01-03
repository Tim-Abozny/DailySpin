namespace DailySpin.DataProvider.Interfaces
{
    public interface IRepository<T>
    {
        bool Create(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        bool Delete(T entity);

    }
}
