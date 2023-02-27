using DailySpin.Logic.Hubs;
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
            //await _glassService.ClearGlasses();
            //_glassService.CreateGlasses();

            var model = await _glassService.GetGlasses();
            RouletteViewModel roulette = new RouletteViewModel();
            roulette.ModelList = new List<BetsGlassViewModel>();
            foreach (var item in model.Data)
            {
                roulette.ModelList.Add(item);
            }

            return View(roulette);
        }

    }
}