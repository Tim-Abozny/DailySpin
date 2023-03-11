using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Repository;

namespace DailySpin.DataProvider
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IUserRepository _userRepository;
        private IBetRepository _betRepository;
        private IBetGlassRepository _betGlassRepository;
        private IRouletteRepository _rouletteRepository;
        private IChipRepository _chipRepository;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserRepository UserRepository => _userRepository = _userRepository ?? new UserRepository(_dbContext);
        public IBetRepository BetRepository => _betRepository = _betRepository ?? new BetRepository(_dbContext);

        public IBetGlassRepository BetGlassRepository => _betGlassRepository = _betGlassRepository ?? new BetGlassRepository(_dbContext);

        public IRouletteRepository RouletteRepository => _rouletteRepository = _rouletteRepository ?? new RouletteRepository(_dbContext);

        public IChipRepository ChipRepository => _chipRepository = _chipRepository ?? new ChipRepository(_dbContext);

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Rollback()
        {
            _dbContext.Dispose();
        }
    }
}
