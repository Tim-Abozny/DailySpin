using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;

namespace DailySpin.DataProvider.Repository
{
    public class UserRepository : BaseRepository<UserAccount>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}