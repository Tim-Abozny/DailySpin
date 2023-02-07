using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.Website.Models;

namespace DailySpin.DataProvider.Repository
{
    public class BetRepository : BaseRepository<Bet>, IBetRepository
    {
        public BetRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
