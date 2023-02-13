using DailySpin.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DailySpin.Website.Views.Shared.Components
{
    public class LoadUserImage : ViewComponent
    {
        public string Invoke()
        {
            var request = HttpContext.RequestServices.
                GetService<IAccountService>()
                .LoadUserData(HttpContext.User.Identity.Name);

            if (request.Result.Data == null)
                return "noImg";
            var model = request.Result.Data.UserImage;
            var retStr = string.Format("data:image/png;base64, {0}", Convert.ToBase64String(model));

            return retStr;
        }
    }
}
