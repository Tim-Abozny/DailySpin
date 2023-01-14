using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.Logic.Interfaces;
using DailySpin.Website.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySpin.Logic.Services
{
    public class BetService : IBetService
    {
        private static IBaseRepository<Bet> _betRepository;
        private readonly ILogger _logger;
        public BetService(IBaseRepository<Bet> betRepository,
            ILogger<BetsGlassService> logger)
        {
            _betRepository = betRepository;
            _logger = logger;
        }
    }
}
