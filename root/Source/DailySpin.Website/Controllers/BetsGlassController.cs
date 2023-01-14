using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;
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
        public async Task<IActionResult> PlaceBet(ChipColor glassColor, uint bet)
        { // incorrect data here (only Green color recived from View) | I'll try to fix it, but API work correctly
            if (bet > 0 && HttpContext.User.Identity.Name != null)
                await _glassService.PlaceBet(glassColor, HttpContext.User.Identity.Name, bet);
            return RedirectToAction("Index");
        }
    }
}
