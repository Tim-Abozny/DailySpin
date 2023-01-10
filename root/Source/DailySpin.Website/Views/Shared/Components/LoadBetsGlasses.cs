using Microsoft.AspNetCore.Mvc;
using DailySpin.Logic.Interfaces;

namespace DailySpin.Website.Views.Shared.Components
{
    public class LoadBetsGlasses : ViewComponent
    {
        public string Invoke()
        {
            var request = HttpContext.RequestServices.
                GetService<IBetsGlassService>()
                .GetGlasses();

            var model = request.Result.Data;
            

            return "";
        }
    }
}
