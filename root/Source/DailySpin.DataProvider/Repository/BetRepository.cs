using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.Website.Models;

namespace DailySpin.DataProvider.Repository
{
    public class BetRepository : IBaseRepository<Bet>
    {
        private readonly AppDbContext _db;
        public BetRepository(AppDbContext db) => _db = db;
        public IQueryable<Bet> GetAll() => _db.Bets;
        public async Task Create(Bet entity)
        {
            await _db.Bets.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(Bet entity)
        {
            _db.Bets.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<Bet> Update(Bet entity)
        {
            _db.Bets.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
