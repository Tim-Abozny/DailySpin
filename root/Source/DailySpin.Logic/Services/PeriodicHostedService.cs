using DailySpin.Logic.Hubs;
using DailySpin.Logic.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DailySpin.Logic.Services
{
    public class PeriodicHostedService : BackgroundService
    {
        private readonly TimeSpan _period = TimeSpan.FromSeconds(15);
        private readonly ILogger<PeriodicHostedService> _logger;
        private readonly IServiceScopeFactory _factory;
        private readonly IHubContext<RouletteHub> _rouletteHub;
        private int _executionCount = 0;
        public bool IsEnabled { get; set; }
        public PeriodicHostedService(ILogger<PeriodicHostedService> logger,
            IServiceScopeFactory factory,
            IHubContext<RouletteHub> rouletteHub)
        {
            _logger = logger;
            _factory = factory;
            IsEnabled = true;
            _rouletteHub = rouletteHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (!stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                    await RouletteHub.GenerateItems();
                    await _rouletteHub.Clients.All.SendAsync("Spin");
                    _executionCount++;
                    _logger.LogInformation($"Executed PeriodicHostedService - Count: {_executionCount}");

                }
                catch (Exception)
                {
                    _logger.LogInformation($"Failed to execute PeriodicHostedService.");
                }
            }
        }
    }
}
