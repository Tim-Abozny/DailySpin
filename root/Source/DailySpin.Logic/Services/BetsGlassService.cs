using DailySpin.DataProvider.Enums;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Response;
using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using DailySpin.Website.Enums;
using DailySpin.Website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DailySpin.Logic.Services
{
    public class BetsGlassService : IBetsGlassService
    {
        private static IBaseRepository<BetsGlass> _glassRepository;
        private readonly ILogger _logger;
        public BetsGlassService(IBaseRepository<BetsGlass> glassRepository,
            ILogger<BetsGlassService> logger)
        {
            _glassRepository = glassRepository;
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
            var cpBets = new List<BetViewModel>();
            

            foreach (var item in list)
            {
                
                retModel.Add(
                new BetsGlassViewModel()
                {
                    BetMultiply = item.BetMultiply,
                    GlassImage = item.GlassImage,
                    ColorType = item.ColorType.ToString(),
                    Bets = item.Bets,
                    TotalBetSum = item.TotalBetSum
                }
                );
            }

            return new BaseResponse<List<BetsGlassViewModel>>()
            {
                Data = retModel,
                StatusCode = StatusCode.OK,
                Description = "Successfully returned list"
            };
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
