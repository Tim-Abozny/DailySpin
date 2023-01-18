using DailySpin.DataProvider.Response;

namespace DailySpin.Logic.Interfaces
{
    public interface IRouletteService
    {
        Task<BaseResponse<bool>> RunAsync();
    }
}
