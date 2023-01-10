using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DailySpin.Website.Views.Shared.Components
{
    public class LoginedUserData : ViewComponent
    {
        public string Invoke()
        {
            var request = HttpContext.RequestServices.
                GetService<IAccountService>()
                .LoadUserData(HttpContext.User.Identity.Name);

            BetViewModel model = request.Result.Data;
            return $"{model.UserName} | Balance: {model.UserBalance} 💰";
        }
    }
}
