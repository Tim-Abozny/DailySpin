using DailySpin.Logic.Hubs;
using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthorizeAttribute = Microsoft.AspNet.SignalR.AuthorizeAttribute;

namespace DailySpin.Website.Controllers
{
    public class BetsGlassController : Controller
    {
        private readonly IBetsGlassService _glassService;
        private readonly IHubContext _hubContext;
        public BetsGlassController(IBetsGlassService glassService)
        {
            _glassService = glassService;
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<RouletteHub>();
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

            var response = _hubContext.Clients.All.PlaceBet(color, HttpContext.User.Identity.Name, bet);
            if (response.Data == false)
            {
                TempData["Message"] = response.Description;
            }
            return RedirectToAction("Index");
        }
    }
}
