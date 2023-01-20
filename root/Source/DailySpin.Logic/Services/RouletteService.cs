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
            
            ushort lowerCounter = 0; // possible to be 0 always, 'cause method reset this var every call
            // need to create Roulete model and contain this var as property
            
            ulong blueBetsSum = (ulong)blueBets.Sum(x => x.UserBet);
            ulong greenBetsSum = (ulong)greenBets.Sum(x => x.UserBet);
            ulong yellowBetsSum = (ulong)yellowBets.Sum(x => x.UserBet);

            ulong minSum = Math.Min(blueBetsSum, Math.Min(greenBetsSum, yellowBetsSum));
            ulong maxSum = Math.Max(blueBetsSum, Math.Max(greenBetsSum, yellowBetsSum));
            ulong minYellowBlueSum = Math.Min(blueBetsSum, yellowBetsSum);

            //this logic will be used for others color. I'll add smth and edit
            if (lowerCounter < 6)
            {
                if (minYellowBlueSum == blueBetsSum) //minSum == minYellowBlueSum && 
                {
                    if (yellowBetsSum - blueBetsSum <= blueBetsSum / 20) //(5%) difference between bets
                    {
                        lowerCounter++;
                    }
                    if (blueBetsSum * 2 < yellowBetsSum + greenBetsSum)
                    {
                        // blue winner
                        foreach (var bet in blueBets)
                        {
                            var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                            user.Balance += bet.UserBet * 2;
                        }
                    }
                    if (blueBetsSum + yellowBetsSum < greenBetsSum * 14)
                    {
                        // yellow winner 
                        foreach (var bet in yellowBets)
                        {
                            var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                            user.Balance += bet.UserBet * 2;
                        }
                    }
                    else if (blueBetsSum == yellowBetsSum)
                    {
                        Random random = new Random();
                        int tempRandValue = random.Next(1, 2);
                        if (tempRandValue == 1)
                        {
                            // blue winner
                            foreach (var bet in blueBets)
                            {
                                var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                                user.Balance += bet.UserBet * 2;
                            }
                        }
                        else if (tempRandValue == 2)
                        {
                            // green winner
                            foreach (var bet in greenBets)
                            {
                                var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                                user.Balance += bet.UserBet * 14;
                            }
                        }
                    }
                }
                else if (minYellowBlueSum == yellowBetsSum) // minSum == minYellowBlueSum && 
                {
                    if (blueBetsSum - yellowBetsSum <= yellowBetsSum / 20)
                    {
                        lowerCounter++;
                    }
                    if (yellowBetsSum * 2 < blueBetsSum + greenBetsSum)
                    {
                        // yellow winner
                        foreach (var bet in yellowBets)
                        {
                            var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                            user.Balance += bet.UserBet * 2;
                        }
                    }
                }
            }
            else
            {
                if (maxSum < 5000)
                {
                    Random random = new Random();
                    int tempRandValue = random.Next(1, 3);
                    if (tempRandValue == 1)
                    {
                        // blue winner
                        foreach (var bet in blueBets)
                        {
                            var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                            user.Balance += bet.UserBet * 2;
                        }
                    }
                    else if (tempRandValue == 2)
                    {
                        // green winner
                        foreach (var bet in greenBets)
                        {
                            var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                            user.Balance += bet.UserBet * 14;
                        }
                    }
                    else
                    {
                        // yellow winner
                        foreach (var bet in yellowBets)
                        {
                            var user = _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == bet.UserAccountId).Result;
                            user.Balance += bet.UserBet * 2;
                        }
                    }
                }
                else
                {

                }
            }
            return new BaseResponse<bool>()
            {
                Data = true, 
                StatusCode = DataProvider.Enums.StatusCode.OK,
                Description = "SuccessfullyRunApp"
            };
        }
    }
}
