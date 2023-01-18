using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.Website.Models;

namespace DailySpin.DataProvider.Repository
{
    public class BetGlassRepository : IBaseRepository<BetsGlass>
    {
        private readonly AppDbContext _db;
        public BetGlassRepository(AppDbContext db) => _db = db;
        public IQueryable<BetsGlass> GetAll() => _db.BetsGlasses;
        public async Task Create(BetsGlass entity)
        {
            await _db.BetsGlasses.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(BetsGlass entity)
        {
            _db.BetsGlasses.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<BetsGlass> Update(BetsGlass entity)
        {
            _db.BetsGlasses.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
