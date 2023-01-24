using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Models;
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
        private static IBaseRepository<Roulette> _rouletteRepository;
        private readonly ILogger _logger;
        public RouletteService(IBaseRepository<BetsGlass> glassRepository,
            IBaseRepository<UserAccount> userRepository,
            IBaseRepository<Bet> betRepository,
            IBaseRepository<Roulette> rouletteRepository,
            ILogger<RouletteService> logger)
        {
            _rouletteRepository = rouletteRepository;
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
            var roulette = await _rouletteRepository.GetAll().FirstOrDefaultAsync();
            ulong blueBetsSum = (ulong)blueBets.Sum(x => x.UserBet) * 2;
            ulong greenBetsSum = (ulong)greenBets.Sum(x => x.UserBet) * 14;
            ulong yellowBetsSum = (ulong)yellowBets.Sum(x => x.UserBet) * 2;
            ulong minSum = Math.Min(blueBetsSum, Math.Min(greenBetsSum, yellowBetsSum));
            ulong maxSum = Math.Max(blueBetsSum, Math.Max(greenBetsSum, yellowBetsSum));
            ulong midSum = greenBetsSum + yellowBetsSum + blueBetsSum - maxSum - minSum;

            if (midSum - minSum >= minSum / 20) // !(5%) difference between bets
            {
                if (minSum == greenBetsSum)
                {
                    // green winner
                    foreach (var bet in greenBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                        user.Balance += bet.UserBet * 14;
                        roulette.Balance -= bet.UserBet * 14;
                        await _userRepository.Update(user);
                    }
                    await _rouletteRepository.Update(roulette);
                }
                else if (minSum == blueBetsSum)
                {
                    // blue winner
                    foreach (var bet in blueBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                        user.Balance += bet.UserBet * 2;
                        roulette.Balance -= bet.UserBet * 2;
                        await _userRepository.Update(user);
                    }
                    await _rouletteRepository.Update(roulette);
                }
                else
                {
                    // yellow winner
                    foreach (var bet in yellowBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                        user.Balance += bet.UserBet * 2;
                        roulette.Balance -= bet.UserBet * 2;
                        await _userRepository.Update(user);
                    }
                    await _rouletteRepository.Update(roulette);
                }
            }
            else
            {
                if (midSum == greenBetsSum)
                {
                    // green winner
                    foreach (var bet in greenBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                        user.Balance += bet.UserBet * 14;
                        await _userRepository.Update(user);
                    }
                    await _rouletteRepository.Update(roulette);
                }
                else if (midSum == blueBetsSum)
                {
                    // blue winner
                    foreach (var bet in blueBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                        user.Balance += bet.UserBet * 2;
                        await _userRepository.Update(user);
                    }
                    await _rouletteRepository.Update(roulette);
                }
                else if (midSum == yellowBetsSum)
                {
                    // yellow winner
                    foreach (var bet in yellowBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                        user.Balance += bet.UserBet * 2;
                        await _userRepository.Update(user);
                    }
                    await _rouletteRepository.Update(roulette);
                }
            }

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = DataProvider.Enums.StatusCode.OK,
                Description = "SuccessfullySpin"
            };
        }
    }
}
