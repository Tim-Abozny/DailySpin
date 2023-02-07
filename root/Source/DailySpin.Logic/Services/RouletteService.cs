using DailySpin.DataProvider;
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
        public IUnitOfWork _unitOfWork;
        private readonly ILogger<RouletteService> _logger;
        public RouletteService(IUnitOfWork unitOfWork,
            ILogger<RouletteService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<BaseResponse<bool>> RunAsync()
        {
            var dbBets = _unitOfWork.BetRepository.GetAll();
            var dbGlasses = _unitOfWork.BetGlassRepository.GetAll();
            Guid colorBlueID = Guid.Empty;
            Guid colorGreenID = Guid.Empty;
            Guid colorYellowID = Guid.Empty;
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
            var roulette = await _unitOfWork.RouletteRepository.GetAll().SingleAsync();
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
                    var greenImage = await _unitOfWork.BetGlassRepository.GetAll().FirstAsync(x => x.ColorType == ChipColor.Green);
                    await AddWinChipAsync(ChipColor.Green, greenImage.GlassImage);
                    foreach (var bet in greenBets)
                    {
                        var user = _unitOfWork.UserRepository.GetAll().FirstAsync(x => x.Id == bet.UserAccountId).Result;
                        user.Balance += bet.UserBet * 14;
                        roulette.Balance -= bet.UserBet * 14;
                        _unitOfWork.UserRepository.Update(user);
                    }
                    _unitOfWork.RouletteRepository.Update(roulette);
                }
                else if (minSum == blueBetsSum)
                {
                    // blue winner
                    var blueImage = await _unitOfWork.BetGlassRepository.GetAll().FirstAsync(x => x.ColorType == ChipColor.Blue);
                    await AddWinChipAsync(ChipColor.Blue, blueImage.GlassImage);
                    foreach (var bet in blueBets)
                    {
                        var user = _unitOfWork.UserRepository.GetAll().First(x => x.Id == bet.UserAccountId);
                        user.Balance += bet.UserBet * 2;
                        roulette.Balance -= bet.UserBet * 2;
                        _unitOfWork.UserRepository.Update(user);
                    }
                    _unitOfWork.RouletteRepository.Update(roulette);
                }
                else
                {
                    // yellow winner
                    var yellowImage = await _unitOfWork.BetGlassRepository.GetAll().FirstAsync(x => x.ColorType == ChipColor.Yellow);
                    await AddWinChipAsync(ChipColor.Yellow, yellowImage.GlassImage);
                    foreach (var bet in yellowBets)
                    {
                        var user = _unitOfWork.UserRepository.GetAll().First(x => x.Id == bet.UserAccountId);
                        user.Balance += bet.UserBet * 2;
                        roulette.Balance -= bet.UserBet * 2;
                        _unitOfWork.UserRepository.Update(user);
                    }
                    _unitOfWork.RouletteRepository.Update(roulette);
                }
            }
            else
            {
                if (midSum == greenBetsSum)
                {
                    // green winner
                    var greenImage = await _unitOfWork.BetGlassRepository.GetAll().FirstAsync(x => x.ColorType == ChipColor.Green);
                    await AddWinChipAsync(ChipColor.Green, greenImage.GlassImage);
                    foreach (var bet in greenBets)
                    {
                        var user = _unitOfWork.UserRepository.GetAll().First(x => x.Id == bet.UserAccountId);
                        user.Balance += bet.UserBet * 14;
                        _unitOfWork.UserRepository.Update(user);
                    }
                    _unitOfWork.RouletteRepository.Update(roulette);
                }
                else if (midSum == blueBetsSum)
                {
                    // blue winner
                    var blueImage = await _unitOfWork.BetGlassRepository.GetAll().FirstAsync(x => x.ColorType == ChipColor.Blue);
                    await AddWinChipAsync(ChipColor.Blue, blueImage.GlassImage);
                    foreach (var bet in blueBets)
                    {
                        var user = _unitOfWork.UserRepository.GetAll().FirstAsync(x => x.Id == bet.UserAccountId).Result;
                        user.Balance += bet.UserBet * 2;
                        _unitOfWork.UserRepository.Update(user);
                    }
                    _unitOfWork.RouletteRepository.Update(roulette);
                }
                else if (midSum == yellowBetsSum)
                {
                    // yellow winner
                    var yellowImage = await _unitOfWork.BetGlassRepository.GetAll().FirstAsync(x => x.ColorType == ChipColor.Yellow);
                    await AddWinChipAsync(ChipColor.Yellow, yellowImage.GlassImage);
                    foreach (var bet in yellowBets)
                    {
                        var user = _unitOfWork.UserRepository.GetAll().First(x => x.Id == bet.UserAccountId);
                        user.Balance += bet.UserBet * 2;
                        _unitOfWork.UserRepository.Update(user);
                    }
                    _unitOfWork.RouletteRepository.Update(roulette);
                }
            }
            await ClearBetsAsync(); // string builder
            _logger.LogInformation($"This spin was successfull");
            _logger.LogInformation($"-----------------------------------------");
            _logger.LogInformation($"blueBets.Count :\t{blueBets.Count}");
            _logger.LogInformation($"greenBets.Count :\t{greenBets.Count}");
            _logger.LogInformation($"yellowBets.Count :\t{yellowBets.Count}");
            _logger.LogInformation($"-----------------------------------------");
            _logger.LogInformation($"dbBets.Count :\t{dbBets.Count()}");
            _logger.LogInformation($"-----------------------------------------");
            _unitOfWork.Commit();
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = DataProvider.Enums.StatusCode.OK,
                Description = "SuccessfullySpin"
            };
        }
        private async Task ClearBetsAsync()
        {
            var bets = _unitOfWork.BetRepository.GetAll();
            foreach (var bet in bets)
            {
                _unitOfWork.BetRepository.Delete(bet);
            }
        }
        private async Task AddWinChipAsync(ChipColor chipColor, byte[] image)
        {
            Chip chip = new Chip
            {
                Id = Guid.NewGuid(),
                ColorType = chipColor,
                Image = image,
                WinChip = true
            };
            _unitOfWork.ChipRepository.Create(chip);
        }
    }
}
