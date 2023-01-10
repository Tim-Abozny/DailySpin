using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DailySpin.Website.Controllers
{
    public class BetsGlassController : Controller
    {
        private readonly IBetsGlassService _glassService;
        
        public BetsGlassController(IBetsGlassService glassService)
        {
            _glassService = glassService;
        }
        public async Task<IActionResult> Index()
        {
            await _glassService.ClearGlasses(); // run with debag at this point
            await _glassService.CreateGlasses(); // 'cause this rows needs to load
            var model = await _glassService.GetGlasses(); // try to fix it later
            List<BetsGlassViewModel> retModel = new List<BetsGlassViewModel>();
            foreach (var item in model.Data)
            {
                retModel.Add(item);
            }
            return View(retModel);
        }
    }
}
