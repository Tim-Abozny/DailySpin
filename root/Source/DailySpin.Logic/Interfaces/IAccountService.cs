using DailySpin.DataProvider.Response;
using DailySpin.ViewModel.ViewModels;
using System.Security.Claims;

namespace DailySpin.Logic.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);

        Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);
        Task<BaseResponse<BetViewModel>> LoadUserData(string loginedUser);
        Task<BaseResponse<bool>> Deposit(string loginedUser, ulong sum);
        Task<BaseResponse<bool>> Withdraw(string loginedUser, ulong sum);
    }
}