using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Response;
using DailySpin.Logic.Interfaces;
using DailySpin.Website.Enums;
using DailySpin.Website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DailySpin.Logic.Services
{
    public class RouletteService : IRouletteService
    {
        private static IBaseRepository<BetsGlass> _glassRepository;
        private static IBaseRepository<UserAccount> _userRepository;
        private static IBaseRepository<Bet> _betRepository;
        private readonly ILogger _logger;
        public RouletteService(IBaseRepository<BetsGlass> glassRepository,
            IBaseRepository<UserAccount> userRepository,
            IBaseRepository<Bet> betRepository,
            ILogger<RouletteService> logger)
        {
            _glassRepository = glassRepository;
            _userRepository = userRepository;
            _betRepository = betRepository;
            _logger = logger;
        }
        public async Task<BaseResponse<bool>> RunAsync()
        {
            var colorBlueID = _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == ChipColor.Blue).Result.Id;
            var colorGreenID = _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == ChipColor.Green).Result.Id;
            var colorYellowID = _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == ChipColor.Yellow).Result.Id;
            var blueBets = await _betRepository.GetAll().Where(x => x.BetsGlassId == colorBlueID).ToListAsync();
            var greenBets = await _betRepository.GetAll().Where(x => x.BetsGlassId == colorGreenID).ToListAsync();
            var yellowBets = await _betRepository.GetAll().Where(x => x.BetsGlassId == colorYellowID).ToListAsync();

            ulong blueBetsSum = (ulong)blueBets.Sum(x => x.UserBet);
            ulong greenBetsSum = (ulong)greenBets.Sum(x => x.UserBet);
            ulong yellowBetsSum = (ulong)yellowBets.Sum(x => x.UserBet);

            ulong minSum = Math.Min(blueBetsSum, Math.Min(greenBetsSum, yellowBetsSum));
            ulong minYellowBlueSum = Math.Min(blueBetsSum, yellowBetsSum);

            //need to add here lowerCounter
            //need to add here if-else with percent (5%) difference between bets
            //this logic will be used for others color. I'll add smth and edit
            if (minSum == minYellowBlueSum && minYellowBlueSum == blueBetsSum)
            {
                if (blueBetsSum * 2 < yellowBetsSum + greenBetsSum)
                {
                    // blue winner
                    foreach (var bet in blueBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                        user.Balance += bet.UserBet * 2;
                    }
                }

            }
            else if (minSum == minYellowBlueSum && minYellowBlueSum == yellowBetsSum)
            {
                if (yellowBetsSum * 2 < blueBetsSum + greenBetsSum)
                {
                    // yellow winner
                }
            }
        }
    }
}
