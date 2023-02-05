using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailySpin.Website.Controllers
{
    public class BetsGlassController : Controller
    {
        private readonly IBetsGlassService _glassService;
        private readonly IRouletteService _rouletteService;

        public BetsGlassController(IBetsGlassService glassService,
            IRouletteService rouletteService)
        {
            _glassService = glassService;
            _rouletteService = rouletteService;
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
            if (TempData["Message"] != null)
                roulette.Exception = (string)TempData["Message"];

            return View(roulette);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceBet(string glassColor, uint bet)
        {
            ChipColor color = ChipColor.Blue;
            if (glassColor == "Yellow")
                color = ChipColor.Yellow;
            else if (glassColor == "Green")
                color = ChipColor.Green;
            var response = await _glassService.PlaceBet(color, HttpContext.User.Identity.Name, bet);

            if (response.Data == false)
            {
                ModelState.AddModelError("", response.Description);
                TempData["Message"] = response.Description;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
