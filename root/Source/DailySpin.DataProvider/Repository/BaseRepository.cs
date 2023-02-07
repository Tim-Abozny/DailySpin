using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DailySpin.DataProvider.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;
        private readonly DbSet<T> _entitySet;
        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _entitySet = dbContext.Set<T>();
        }
        public void Create(T entity) => _dbContext.Add(entity);

        public void Delete(T entity) => _dbContext.Remove(entity);

        public IQueryable<T> GetAll() => _entitySet.AsQueryable();

        public void Update(T entity) => _dbContext.Update(entity);
    }
}
