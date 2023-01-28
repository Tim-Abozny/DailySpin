using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.Website.Models;

namespace DailySpin.DataProvider.Repository
{
    public class ChipRepository : IBaseRepository<Chip>
    {
        private readonly AppDbContext _db;
        public ChipRepository(AppDbContext db) => _db = db;
        public IQueryable<Chip> GetAll() => _db.Chips;
        public async Task Create(Chip entity)
        {
            await _db.Chips.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(Chip entity)
        {
            _db.Chips.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<Chip> Update(Chip entity)
        {
            _db.Chips.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
