using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.Website.Models;

namespace DailySpin.DataProvider.Repository
{
    public class ChipRepository : BaseRepository<Chip>, IChipRepository
    {
        public ChipRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
