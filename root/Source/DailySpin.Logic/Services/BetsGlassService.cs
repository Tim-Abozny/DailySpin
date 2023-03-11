using DailySpin.DataProvider;
using DailySpin.DataProvider.Enums;
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
        private const ushort betsGlassCounter = 4;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private IWebHostEnvironment _environment;
        public BetsGlassService(IUnitOfWork unitOfWork,
            IWebHostEnvironment environment,
            ILogger<BetsGlassService> logger)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> ClearGlasses()
        {
            BetsGlass betsGlass;
            for (int i = 0; i < betsGlassCounter; i++)
            {
                betsGlass = await _unitOfWork.BetGlassRepository.GetAll().FirstAsync();
                if (betsGlass != null)
                    _unitOfWork.BetGlassRepository.Delete(betsGlass);
            }
            _unitOfWork.Commit();
            return new BaseResponse<bool>()
            {
                Data = true
            };
        }
        public async Task<BaseResponse<List<BetsGlassViewModel>>> GetGlasses()
        {
            var list = await _unitOfWork.BetGlassRepository.GetAll().ToListAsync();
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
                var retBet = _unitOfWork.BetRepository.GetAll().Where(x => x.BetsGlassId == item.Id).ToList();
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
            _unitOfWork.Commit();
            return new BaseResponse<List<BetsGlassViewModel>>()
            {
                Data = retModel,
                StatusCode = StatusCode.OK,
                Description = "Successfully returned list"
            };
        }
        public async Task<BaseResponse<bool>> PlaceBet(ChipColor glassColor, string loginedUsername, uint bet)
        {

            var user = await _unitOfWork.UserRepository.GetAll().FirstAsync(x => x.DisplayName == loginedUsername);
            if (bet > user.Balance || bet < 1)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = "Bet higher then balance or lower then 1",
                    StatusCode = StatusCode.InternalServerError
                };
            }

            var glass = await _unitOfWork.BetGlassRepository.GetAll().FirstAsync(x => x.ColorType == glassColor);
            var roulette = await _unitOfWork.RouletteRepository.GetAll().FirstAsync();
            if (roulette == null)
            {
                Roulette dbRoulette = new Roulette();
                dbRoulette.Id = Guid.NewGuid();
                dbRoulette.Balance = bet;
                _unitOfWork.RouletteRepository.Create(dbRoulette);
            }
            else
            {
                roulette.Balance += bet;
                _unitOfWork.RouletteRepository.Update(roulette);
            }
            user.Balance -= bet;
            _unitOfWork.UserRepository.Update(user);
            Bet dbBet = new Bet
            {
                Id = Guid.NewGuid(),
                UserAccountId = user.Id,
                UserBet = bet,
                UserImage = user.Image!,
                UserName = user.DisplayName,
                BetsGlassId = glass.Id
            };
            _unitOfWork.BetRepository.Create(dbBet);

            if (glass.Bets == null)
                glass.Bets = new List<Bet>();
            glass.Bets.Add(dbBet);
            _unitOfWork.BetGlassRepository.Update(glass);
            _unitOfWork.Commit();

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Glasses has been created!"
            };
        }
        public BaseResponse<bool> CreateGlasses()
        {

            CreateGlass("blue", 2, ChipColor.Blue);
            CreateGlass("green", 14, ChipColor.Green);
            CreateGlass("yellow", 2, ChipColor.Yellow);
            _unitOfWork.Commit();
            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Glasses has been created!"
            };
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
            _unitOfWork.BetGlassRepository.Create(Glass);
        }
    }
}
