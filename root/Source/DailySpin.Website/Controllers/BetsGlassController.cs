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
            // await _glassService.ClearGlasses();
            // await _glassService.CreateGlasses();
            var model = await _glassService.GetGlasses();
            List<BetsGlassViewModel> retModel = new List<BetsGlassViewModel>();
            foreach (var item in model.Data)
            {
                retModel.Add(item);
            }
            return View(retModel);
        }
        [HttpPost]
        public async Task<IActionResult> PlaceBet(BetsGlassViewModel glassModel, uint bet)
        {
            if (bet > 0)
                await _glassService.PlaceBet(glassModel, HttpContext.User.Identity.Name, bet);
            return View();
        }
    }
}
