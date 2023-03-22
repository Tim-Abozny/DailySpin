using DailySpin.DataProvider;
using DailySpin.Logic.Interfaces;
using DailySpin.Website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DailySpin.Logic.Services
{
    public class HistoryService : IHistoryService
    {
        private const int historyDisplayCount = 6;
        public IUnitOfWork _unitOfWork;
        private readonly ILogger<RouletteService> _logger;
        public HistoryService(IUnitOfWork unitOfWork, ILogger<RouletteService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<Chip>> GetChips()
        {
            var chips = await _unitOfWork.ChipRepository.GetAll().OrderBy(time => time.Date).ToListAsync();
            List<Chip> retChips = new List<Chip>();
            if (chips.Count() >= 5)
            {
                for (int i = chips.Count() - historyDisplayCount; i < chips.Count(); i++)
                {
                    retChips.Add(chips[i]);
                }
            }
            else
            {
                foreach (var chip in chips)
                {
                    retChips.Add(chip);
                }
            }
            _logger.LogInformation("History service successfully completed work.");
            return retChips;
        }
    }
}
