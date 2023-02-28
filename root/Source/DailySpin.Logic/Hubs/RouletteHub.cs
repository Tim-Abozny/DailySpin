using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;
using Microsoft.AspNetCore.SignalR;

namespace DailySpin.Logic.Hubs
{
    public class RouletteHub : Hub
    {
        private readonly IBetsGlassService _glassService;
        private readonly IAccountService _accountService;

        public RouletteHub(IBetsGlassService glassService,
            IAccountService accountService)
        {
            _glassService = glassService;
            _accountService = accountService;
        }

        public async Task PlaceBetf(string color, int bet)
        {
            ChipColor glassColor;
            if (color == "blue")
                glassColor = ChipColor.Blue;
            else if (color == "green")
                glassColor = ChipColor.Green;
            else
                glassColor = ChipColor.Yellow;

            string name = Context.User.Identity.Name;
            var response = await _glassService.PlaceBet(glassColor, name, (uint)bet);
            if (response.Data == false)
            {
                await Clients.Caller.SendAsync("ReturnError", response.Description);
            }
            else
            {
                BetViewModel viewModel = new BetViewModel();
                viewModel.UserBet = (uint)bet;
                viewModel.UserName = name;
                viewModel.UserImage = _accountService.LoadUserData(name).Result.Data.UserImage;

                await Clients.Others.SendAsync("PlaceBet", viewModel, color);
            }
        }
        public async Task Spin()
        {

        }
    }
}
