using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DailySpin.Website.Views.Shared.Components
{
    public class LoginedUserData : ViewComponent
    {
        public string Invoke()
        {
            if (HttpContext.User.Identity.Name == null)
            {
                return "PLEASE LOG IN";
            }
            var request = HttpContext.RequestServices.
                GetService<IAccountService>()
                .LoadUserData(HttpContext.User.Identity.Name);

            if (request.Result.Data == null)
                return "NEED TO LOG IN";
            BetViewModel model = request.Result.Data;
            return $"{model.UserName} | ";
        }
    }
}
