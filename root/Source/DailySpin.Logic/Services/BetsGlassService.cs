using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Enums;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Repository;
using DailySpin.DataProvider.Response;
using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;
using DailySpin.Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DailySpin.Logic.Services
{
    public class BetsGlassService : IBetsGlassService
    {
        private static IBaseRepository<BetsGlass> _glassRepository;
        private static IBaseRepository<UserAccount> _userRepository;
        private static IBaseRepository<Bet> _betRepository;
        private readonly ILogger _logger;
        public BetsGlassService(IBaseRepository<BetsGlass> glassRepository,
            IBaseRepository<UserAccount> userRepository,
            IBaseRepository<Bet> betRepository,
            ILogger<BetsGlassService> logger)
        {
            _glassRepository = glassRepository;
            _userRepository = userRepository;
            _betRepository = betRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> ClearGlasses()
        {
            //solved optimization problem ;)
            BetsGlass betsGlass;
            for (int i = 0; i < 4; i++)
            {
                betsGlass = await _glassRepository.GetAll().FirstOrDefaultAsync<BetsGlass>();
                if (betsGlass != null)
                    await _glassRepository.Delete(betsGlass);
            }
            return new BaseResponse<bool>()
            {
                Data = true
            };
        }

        public async Task<BaseResponse<List<BetsGlassViewModel>>> GetGlasses()
        {
            var list = await _glassRepository.GetAll().ToListAsync();
            if (list == null)
            {
                return new BaseResponse<List<BetsGlassViewModel>>()
                {
                    Data = new List<BetsGlassViewModel>(),
                    StatusCode = StatusCode.OK,
                    Description = "Successfully returned empty list"
                };
            }
            List<BetsGlassViewModel> retModel = new List<BetsGlassViewModel>();

            foreach (var item in list)
            {
                //найди тут все ставки для цвета и присвой возвращаемому объекту. Будет тебе счастье!
                var retBet = _betRepository.GetAll().Where<Bet>(x => x.BetsGlassId == item.Id).ToList();
                if (item.Bets == null)
                {
                    retModel.Add(
                    new BetsGlassViewModel()
                    {
                        BetMultiply = item.BetMultiply,
                        GlassImage = item.GlassImage,
                        ColorType = item.ColorType,
                        Bets = retBet,
                        TotalBetSum = item.TotalBetSum
                    }
                    );
                }
                else
                {
                    retModel.Add(
                    new BetsGlassViewModel()
                    {
                        BetMultiply = item.BetMultiply,
                        GlassImage = item.GlassImage!,
                        ColorType = item.ColorType,
                        Bets = retBet,
                        TotalBetSum = item.TotalBetSum
                    }
                    );
                }
            }

            return new BaseResponse<List<BetsGlassViewModel>>()
            {
                Data = retModel,
                StatusCode = StatusCode.OK,
                Description = "Successfully returned list"
            };
        }

        public async Task<BaseResponse<bool>> PlaceBet(ChipColor glassColor, string loginedUsername, uint bet)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.DisplayName == loginedUsername);
                if (bet <= 0 || bet > user.Balance)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "ERROR while try PlaceBet",
                        StatusCode = StatusCode.InternalServerError
                    };
                }

                var glass = await _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == glassColor);
                
                user.Balance -= bet;
                await _userRepository.Update(user);
                // походу нужно сначала добавить объект Bet в бд, а потом его же в список Bets в BetsGlass
                // для этого ещё понадобится репа _betRep
                Bet dbBet = new Bet
                {
                    Id = Guid.NewGuid(),
                    UserAccountId = user.Id,
                    UserBet = bet,
                    UserImage = user.Image,
                    UserName = user.DisplayName,
                    BetsGlassId = glass.Id
                };
                await _betRepository.Create(dbBet);
                
                if (glass.Bets == null)
                {
                    glass.Bets = new List<Bet>{dbBet};
                    await _glassRepository.Update(glass);
                }
                else
                {
                    glass.Bets.Add(dbBet);
                    await _glassRepository.Update(glass);
                }

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "Glasses has been created!"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[PlaceBet]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<BaseResponse<bool>> CreateGlasses()
        {
            try
            {
                await fastCreateGlassAsync("blue", 2, ChipColor.Blue);
                await fastCreateGlassAsync("green", 14, ChipColor.Green);
                await fastCreateGlassAsync("yellow", 2, ChipColor.Yellow);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "Glasses has been created!"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateGlasses]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private byte[] GetImage(string imgColor)
        {
            byte[] imageArray = File.ReadAllBytes($"C:\\Users\\progr\\source\\repos\\C#\\5sem\\trainee\\root\\Source\\DailySpin.Website\\wwwroot\\img\\{imgColor}Chip.png");
            return imageArray;
        }
        private async Task fastCreateGlassAsync(string colorGlass, ushort betMultiply, ChipColor chipColor)
        {
            BetsGlass Glass = new BetsGlass()
            {
                Id = Guid.NewGuid(),
                BetMultiply = betMultiply,
                Bets = new List<Bet>(),
                BetsCount = 0,
                TotalBetSum = 0,
                GlassImage = GetImage(colorGlass),
                ColorType = chipColor
            };
            await _glassRepository.Create(Glass);
        }
    }
}
