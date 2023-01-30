using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Enums;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Models;
using DailySpin.DataProvider.Response;
using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;
using DailySpin.Website.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DailySpin.Logic.Services
{
    public class BetsGlassService : IBetsGlassService
    {
        private static IBaseRepository<BetsGlass> _glassRepository;
        private static IBaseRepository<UserAccount> _userRepository;
        private static IBaseRepository<Bet> _betRepository;
        private static IBaseRepository<Roulette> _rouletteRepository;
        private readonly ILogger _logger;
        private IWebHostEnvironment _environment;
        public BetsGlassService(IBaseRepository<BetsGlass> glassRepository,
            IBaseRepository<UserAccount> userRepository,
            IBaseRepository<Bet> betRepository,
            IBaseRepository<Roulette> rouletteRepository,
            IWebHostEnvironment environment,
            ILogger<BetsGlassService> logger)
        {
            _rouletteRepository = rouletteRepository;
            _glassRepository = glassRepository;
            _userRepository = userRepository;
            _betRepository = betRepository;
            _environment = environment;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> ClearGlasses()
        {
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
                var retBet = _betRepository.GetAll().Where(x => x.BetsGlassId == item.Id).ToList();
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
                if (bet > user.Balance)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Bet higher then balance",
                        StatusCode = StatusCode.InternalServerError
                    };
                }

                var glass = await _glassRepository.GetAll().FirstOrDefaultAsync(x => x.ColorType == glassColor);
                var roulette = await _rouletteRepository.GetAll().FirstOrDefaultAsync();
                if (roulette == null)
                {
                    Roulette dbRoulette = new Roulette();
                    dbRoulette.Id = Guid.NewGuid();
                    dbRoulette.Balance = bet;
                    await _rouletteRepository.Create(dbRoulette);
                }
                else
                {
                    roulette.Balance += bet;
                    await _rouletteRepository.Update(roulette);
                }
                user.Balance -= bet;
                await _userRepository.Update(user);
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
                    glass.Bets = new List<Bet> { dbBet };
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
        public BaseResponse<bool> CreateGlasses()
        {
            try
            {
                CreateGlass("blue", 2, ChipColor.Blue);
                CreateGlass("green", 14, ChipColor.Green);
                CreateGlass("yellow", 2, ChipColor.Yellow);

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
            string wwwPath = _environment.WebRootPath;
            byte[] imageArray = File.ReadAllBytes($"{wwwPath}\\img\\{imgColor}Chip.png");
            return imageArray;
        }
        private void CreateGlass(string colorGlass, ushort betMultiply, ChipColor chipColor)
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
            _glassRepository.Create(Glass);
        }
    }
}
