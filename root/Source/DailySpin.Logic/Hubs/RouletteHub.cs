using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;

namespace DailySpin.Logic.Hubs
{
    public class RouletteHub : Hub
    {
        private readonly IBetsGlassService _glassService;
        private readonly IAccountService _accountService;
        private static readonly List<Item> Items = new List<Item>
        {
            new Item { Name = "GreenChip", Image = "img/greenChip.png", Chance = 2 },
            new Item { Name = "YellowChip", Image = "img/yellowChip.png", Chance = 50 },
            new Item { Name = "BlueChip", Image = "img/blueChip.png", Chance = 100 }
        };
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

                await Clients.All.SendAsync("PlaceBet", viewModel, color);
            }
        }
        public async Task<Item> GetItem()
        {
            Item item = null;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            while (item == null)
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                int chance = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 100;

                foreach (var elm in Items)
                {
                    if (chance < elm.Chance && item == null)
                        item = elm;
                }
            }

            return item;
        }
        public async Task Spin()
        {

        }
    }
}
