using DailySpin.DataProvider.Response;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;

namespace DailySpin.Logic.Interfaces
{
    public interface IBetsGlassService
    {
        BaseResponse<bool> CreateGlasses();
        Task<BaseResponse<bool>> ClearGlasses();
        Task<BaseResponse<bool>> PlaceBet(ChipColor glassColor, string loginedUsername, uint bet);
        Task<BaseResponse<List<BetsGlassViewModel>>> GetGlasses();

    }
}
