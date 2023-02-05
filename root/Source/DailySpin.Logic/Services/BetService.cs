using DailySpin.DataProvider.Interfaces;
using DailySpin.Logic.Interfaces;
using DailySpin.Website.Models;
using Microsoft.Extensions.Logging;

namespace DailySpin.Logic.Services
{
    public class BetService : IBetService
    {
        private readonly IBaseRepository<Bet> _betRepository;
        private readonly ILogger _logger;
        public BetService(IBaseRepository<Bet> betRepository,
            ILogger<BetsGlassService> logger)
        {
            _betRepository = betRepository;
            _logger = logger;
        }
    }
}
