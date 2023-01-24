using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Models;

namespace DailySpin.DataProvider.Repository
{
    public class RouletteRepository : IBaseRepository<Roulette>
    {
        private readonly AppDbContext _db;
        public RouletteRepository(AppDbContext db) => _db = db;
        public IQueryable<Roulette> GetAll() => _db.Roulettes;
        public async Task Create(Roulette entity)
        {
            await _db.Roulettes.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(Roulette entity)
        {
            _db.Roulettes.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<Roulette> Update(Roulette entity)
        {
            _db.Roulettes.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
