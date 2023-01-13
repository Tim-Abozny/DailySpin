using DailySpin.DataProvider.Response;
using DailySpin.ViewModel.ViewModels;
using System.Security.Claims;

namespace DailySpin.Logic.Interfaces
{
    public interface IBetsGlassService
    {
        Task<BaseResponse<bool>> CreateGlasses();
        Task<BaseResponse<bool>> ClearGlasses();
        Task<BaseResponse<bool>> PlaceBet(BetsGlassViewModel glassModel, string loginedUsername, uint bet);
        Task<BaseResponse<List<BetsGlassViewModel>>> GetGlasses();

    }
}
