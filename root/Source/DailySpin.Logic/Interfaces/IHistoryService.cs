using DailySpin.DataProvider.Response;
using DailySpin.Website.Models;

namespace DailySpin.Logic.Interfaces
{
    public interface IHistoryService
    {
        Task<List<Chip>> GetChips();
    }
}
