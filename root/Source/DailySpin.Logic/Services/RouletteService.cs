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
        private readonly IBaseRepository<BetsGlass> _glassRepository;
        private readonly IBaseRepository<UserAccount> _userRepository;
        private readonly IBaseRepository<Bet> _betRepository;
        private readonly IBaseRepository<Roulette> _rouletteRepository;
        private readonly IBaseRepository<Chip> _winHistoryRepository;
        private readonly ILogger<RouletteService> _logger;
        public RouletteService(IBaseRepository<BetsGlass> glassRepository,
            IBaseRepository<UserAccount> userRepository,
            IBaseRepository<Bet> betRepository,
            IBaseRepository<Roulette> rouletteRepository,
            IBaseRepository<Chip> winHistory,
            ILogger<RouletteService> logger)
        {
            _rouletteRepository = rouletteRepository;
            _glassRepository = glassRepository;
            _userRepository = userRepository;
            _betRepository = betRepository;
            _winHistoryRepository = winHistory;
            _logger = logger;
        }
        public async Task<BaseResponse<bool>> RunAsync()
        {
            var dbBets = _betRepository.GetAll();
            var dbGlasses = _glassRepository.GetAll();
            Guid colorBlueID = Guid.Empty;   // = _glassRepository.GetAll().FirstOrDefault(x => x.ColorType == ChipColor.Blue).Id;
            Guid colorGreenID = Guid.Empty;  // = _glassRepository.GetAll().FirstOrDefault(x => x.ColorType == ChipColor.Green).Id;
            Guid colorYellowID = Guid.Empty; // = _glassRepository.GetAll().FirstOrDefault(x => x.ColorType == ChipColor.Yellow).Id;
            foreach (var glass in dbGlasses)
            {
                if (glass.ColorType == ChipColor.Blue)
                {
                    colorBlueID = glass.Id;
                }
                else if (glass.ColorType == ChipColor.Green)
                {
                    colorGreenID = glass.Id;
                }
                else
                {
                    colorYellowID = glass.Id;
                }
            }
            List<Bet> blueBets = dbBets.Where(x => x.BetsGlassId == colorBlueID).ToList();
            List<Bet> greenBets = dbBets.Where(x => x.BetsGlassId == colorGreenID).ToList();
            List<Bet> yellowBets = dbBets.Where(x => x.BetsGlassId == colorYellowID).ToList();
            //var blueBets = await _betRepository.GetAll().Where(x => x.BetsGlassId == colorBlueID).ToListAsync();
            //var greenBets = await _betRepository.GetAll().Where(x => x.BetsGlassId == colorGreenID).ToListAsync();
            //var yellowBets = await _betRepository.GetAll().Where(x => x.BetsGlassId == colorYellowID).ToListAsync();
            if (blueBets.Count == 0 && greenBets.Count == 0 && yellowBets.Count == 0)
            {
                _logger.LogInformation("This spin was empty");
                _logger.LogInformation($"-----------------------------------------");
                _logger.LogInformation($"blueBets.Count :\t{blueBets.Count}");
                _logger.LogInformation($"greenBets.Count :\t{greenBets.Count}");
                _logger.LogInformation($"yellowBets.Count :\t{yellowBets.Count}");
                _logger.LogInformation($"-----------------------------------------");
                _logger.LogInformation($"dbBets.Count :\t{dbBets.Count()}");
                _logger.LogInformation($"-----------------------------------------");
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = DataProvider.Enums.StatusCode.OK,
                    Description = "EmptySpin",
                };
            }
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
                    var greenImage = await _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == ChipColor.Green);
                    await AddToWinHistoryAsync(ChipColor.Green, greenImage.GlassImage);
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
                    var blueImage = await _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == ChipColor.Blue);
                    await AddToWinHistoryAsync(ChipColor.Green, blueImage.GlassImage);
                    foreach (var bet in blueBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == bet.UserAccountId);
                        user.Balance += bet.UserBet * 2;
                        roulette.Balance -= bet.UserBet * 2;
                        await _userRepository.Update(user);
                    }
                    await _rouletteRepository.Update(roulette);
                }
                else
                {
                    // yellow winner
                    var yellowImage = await _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == ChipColor.Yellow);
                    await AddToWinHistoryAsync(ChipColor.Green, yellowImage.GlassImage);
                    foreach (var bet in yellowBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == bet.UserAccountId);
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
                    var greenImage = await _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == ChipColor.Green);
                    await AddToWinHistoryAsync(ChipColor.Green, greenImage.GlassImage);
                    foreach (var bet in greenBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == bet.UserAccountId);
                        user.Balance += bet.UserBet * 14;
                        await _userRepository.Update(user);
                    }
                    await _rouletteRepository.Update(roulette);
                }
                else if (midSum == blueBetsSum)
                {
                    // blue winner
                    var blueImage = await _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == ChipColor.Blue);
                    await AddToWinHistoryAsync(ChipColor.Green, blueImage.GlassImage);
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
                    var yellowImage = await _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == ChipColor.Yellow);
                    await AddToWinHistoryAsync(ChipColor.Green, yellowImage.GlassImage);
                    foreach (var bet in yellowBets)
                    {
                        var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == bet.UserAccountId);
                        user.Balance += bet.UserBet * 2;
                        await _userRepository.Update(user);
                    }
                    await _rouletteRepository.Update(roulette);
                }
            }
            await ClearBetsAsync();
            _logger.LogInformation($"This spin was successfull");
            _logger.LogInformation($"-----------------------------------------");
            _logger.LogInformation($"blueBets.Count :\t{blueBets.Count}");
            _logger.LogInformation($"greenBets.Count :\t{greenBets.Count}");
            _logger.LogInformation($"yellowBets.Count :\t{yellowBets.Count}");
            _logger.LogInformation($"-----------------------------------------");
            _logger.LogInformation($"dbBets.Count :\t{dbBets.Count()}");
            _logger.LogInformation($"-----------------------------------------");
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = DataProvider.Enums.StatusCode.OK,
                Description = "SuccessfullySpin"
            };
        }
        private async Task ClearBetsAsync()
        {
            var bets = _betRepository.GetAll();
            foreach (var bet in bets)
            {
                await _betRepository.Delete(bet);
            }
        }
        private async Task AddToWinHistoryAsync(ChipColor chipColor, byte[] image)
        {
            Chip chip = new Chip
            {
                Id = Guid.NewGuid(),
                ColorType = chipColor,
                Image = image,
                WinChip = true
            };
            await _winHistoryRepository.Create(chip);
        }
    }
}
