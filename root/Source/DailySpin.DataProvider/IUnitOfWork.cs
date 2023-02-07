using DailySpin.DataProvider.Interfaces;

namespace DailySpin.DataProvider
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IBetRepository BetRepository { get; }
        IBetGlassRepository BetGlassRepository { get; }
        IRouletteRepository RouletteRepository { get; }
        IChipRepository ChipRepository { get; }
        void Commit();
        void Rollback();
    }
}
