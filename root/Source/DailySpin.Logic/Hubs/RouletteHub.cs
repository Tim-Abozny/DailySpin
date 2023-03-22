using DailySpin.Logic.Interfaces;
using DailySpin.Logic.Services;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;
using DailySpin.Website.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;

namespace DailySpin.Logic.Hubs
{
    public class RouletteHub : Hub
    {
        private readonly IBetsGlassService _glassService;
        private readonly IAccountService _accountService;
        private readonly IRouletteService _rouletteService;
        private readonly IHistoryService _historyService;
        private static readonly List<Item> Items = new List<Item>
        {
            new Item { Name = "GreenChip", Image = "img/greenChip.png", Chance = 2 },
            new Item { Name = "YellowChip", Image = "img/yellowChip.png", Chance = 50 },
            new Item { Name = "BlueChip", Image = "img/blueChip.png", Chance = 100 }
        };
        private static List<Item> items = new List<Item>();

        public RouletteHub(IBetsGlassService glassService,
            IAccountService accountService,
            IRouletteService rouletteService,
            IHistoryService historyService)
        {
            _glassService = glassService;
            _accountService = accountService;
            _rouletteService = rouletteService;
            _historyService = historyService;
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
        public static async Task<List<Item>> GenerateItems()
        {
            items.Clear();
            Item item;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            for (int i = 0; i < 31; i++)
            {
                item = null!;
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
                items.Add(item);
            }
            return items;
        }
        public async Task<List<Item>> FirstGenerateItems()
        {
            if (items.Count > 0)
            {
                return items;
            }
            Item item;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            for (int i = 0; i < 31; i++)
            {
                item = null!;
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
                items.Add(item);
            }
            return items;
        }
        public async Task<List<Item>> GetItemsList() => items;
        public async Task<List<Item>> GetHistoryList()
        {
            List<Item> retChips = new List<Item>();
            var chips = await _historyService.GetChips();
            foreach (var chip in chips)
            {
                if (chip.ColorType == ChipColor.Blue)
                {
                    retChips.Add(new Item { Name = "BlueChip", Image = "img/blueChip.png", Chance = 0 });
                }
                else if (chip.ColorType == ChipColor.Green)
                {
                    retChips.Add(new Item { Name = "GreenChip", Image = "img/greenChip.png", Chance = 0 });
                }
                else
                {
                    retChips.Add(new Item { Name = "YellowChip", Image = "img/yellowChip.png", Chance = 0 });
                }
            }
            return retChips;
        }

        public async Task Spin() { }
        public async Task<ulong> GetActualBalanceAsync()
        {
            var request = await _accountService.LoadUserData(Context.User.Identity.Name);
            if (request.Description == "No any users")
                return 0;
            return request.Data.UserBalance;
        }
        public async Task RunAsyncServerMethod(string ItemGlobalName)
        {
            await _rouletteService.RunAsync(ItemGlobalName);
        }
    }
}
