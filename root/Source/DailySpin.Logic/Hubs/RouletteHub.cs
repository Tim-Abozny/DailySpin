using DailySpin.DataProvider.Response;
using DailySpin.Logic.Interfaces;
using DailySpin.Website.Enums;
using Microsoft.AspNetCore.SignalR;

namespace DailySpin.Logic.Hubs
{
    public class RouletteHub : Hub
    {
        private readonly IBetsGlassService _glassService;

        public RouletteHub(IBetsGlassService glassService)
        {
            _glassService = glassService;
        }

        public async Task<BaseResponse<bool>> PlaceBet(ChipColor glassColor, string name, uint bet)
        {
            var response = await _glassService.PlaceBet(glassColor, name, bet);
            await Clients.All.SendAsync("PlaceBet", glassColor, bet, name);
            return response;
        }
    }
}
