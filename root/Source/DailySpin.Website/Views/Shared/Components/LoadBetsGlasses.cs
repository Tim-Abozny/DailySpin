using DailySpin.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
