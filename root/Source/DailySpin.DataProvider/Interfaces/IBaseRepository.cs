namespace DailySpin.DataProvider.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        void Create(T entity);

        IQueryable<T> GetAll();

        void Update(T entity);

        void Delete(T entity);
    }
}