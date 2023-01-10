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
        private readonly IBaseRepository<Bet> _bet;
        private readonly ILogger<BetService> _logger;
        public BetService(IBaseRepository<Bet> bet, ILogger<BetService> logger)
        {
            _bet = bet;
            _logger = logger;
        }
    }
}
