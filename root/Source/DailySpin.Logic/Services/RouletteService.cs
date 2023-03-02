using DailySpin.DataProvider;
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
        public IUnitOfWork _unitOfWork;
        private readonly ILogger<RouletteService> _logger;
        public RouletteService(IUnitOfWork unitOfWork,
            ILogger<RouletteService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<BaseResponse<string>> RunAsync()
        {
            string result = "";
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
                result = "empty";
                return new BaseResponse<string>()
                {
                    Data = result,
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
                    SetWinner(ChipColor.Green, greenBets, roulette, 14);
                    result = "green";
                }
                else if (minSum == blueBetsSum)
                {
                    // blue winner
                    SetWinner(ChipColor.Blue, blueBets, roulette, 2);
                    result = "blue";
                }
                else
                {
                    // yellow winner
                    SetWinner(ChipColor.Yellow, yellowBets, roulette, 2);
                    result = "yellow";
                }
            }
            else
            {
                if (midSum == greenBetsSum)
                {
                    // green winner
                    SetWinner(ChipColor.Green, greenBets, roulette, 14);
                    result = "green";
                }
                else if (midSum == blueBetsSum)
                {
                    // blue winner
                    SetWinner(ChipColor.Blue, blueBets, roulette, 2);
                    result = "blue";
                }
                else if (midSum == yellowBetsSum)
                {
                    // yellow winner
                    SetWinner(ChipColor.Yellow, yellowBets, roulette, 2);
                    result = "yellow";
                }
            }
            ClearBets(); // string builder
            _unitOfWork.Commit();
            return new BaseResponse<string>()
            {
                Data = result,
                StatusCode = DataProvider.Enums.StatusCode.OK,
                Description = "SuccessfullySpin"
            };
        }
        private void ClearBets()
        {
            var bets = _unitOfWork.BetRepository.GetAll();
            foreach (var bet in bets)
            {
                _unitOfWork.BetRepository.Delete(bet);
            }
        }
        private void AddWinChip(ChipColor chipColor, byte[] image)
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
        private void SetWinner(ChipColor color, List<Bet> coloredBets, Roulette roulette, ushort betMultiply)
        {
            var coloredImage = _unitOfWork.BetGlassRepository.GetAll().FirstAsync(x => x.ColorType == color).Result;
            AddWinChip(color, coloredImage.GlassImage!);
            foreach (var bet in coloredBets)
            {
                var user = _unitOfWork.UserRepository.GetAll().FirstAsync(x => x.Id == bet.UserAccountId).Result;
                user.Balance += bet.UserBet * betMultiply;
                roulette.Balance -= bet.UserBet * betMultiply;
                _unitOfWork.UserRepository.Update(user);
            }
            _unitOfWork.RouletteRepository.Update(roulette);
        }
    }
}
