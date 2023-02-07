using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Models;

namespace DailySpin.DataProvider.Repository
{
    public class RouletteRepository : BaseRepository<Roulette>, IRouletteRepository
    {
        public RouletteRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
