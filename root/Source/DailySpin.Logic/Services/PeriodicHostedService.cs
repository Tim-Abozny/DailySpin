using DailySpin.Logic.Interfaces;
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
        private int _executionCount = 0;
        public bool IsEnabled { get; set; }
        public PeriodicHostedService(ILogger<PeriodicHostedService> logger,
            IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
            IsEnabled = true;
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
                    IRouletteService rouletteService = asyncScope.ServiceProvider.GetRequiredService<IRouletteService>();
                    await rouletteService.RunAsync();
                    _executionCount++;
                    _logger.LogInformation($"Executed PeriodicHostedService - Count: {_executionCount}");

                }
                catch (Exception ex)
                {

                    _logger.LogInformation($"Failed to execute PeriodicHostedService with exception message {ex.Message}.");
                }
            }
        }
    }
}
