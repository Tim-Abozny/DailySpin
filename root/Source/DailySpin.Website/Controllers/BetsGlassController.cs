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
            await _glassService.ClearGlasses();
            _glassService.CreateGlasses();

            var model = await _glassService.GetGlasses();
            List<BetsGlassViewModel> retModel = new List<BetsGlassViewModel>();
            foreach (var item in model.Data)
            {
                retModel.Add(item);
            }
            //await _rouletteService.RunAsync(); use it like background task
            return View(retModel);
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
                return View(response.Description);
            } // need to return view with model's error
            return RedirectToAction("Index");
        }
    }
}
