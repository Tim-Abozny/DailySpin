using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DailySpin.Website.Views.Shared.Components
{
    public class LoginedUserBalance : ViewComponent
    {
        public string Invoke()
        {
            var request = HttpContext.RequestServices.
                GetService<IAccountService>()
                .LoadUserData(HttpContext.User.Identity.Name);

            if (request.Result.Data == null)
                return "NEED TO LOG IN";
            BetViewModel model = request.Result.Data;
            return $"{model.UserBalance}";
        }
    }
}
// 💰