using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Models;
using DailySpin.DataProvider.Repository;
using DailySpin.Logic.Interfaces;
using DailySpin.Logic.Services;
using DailySpin.Website.Models;

namespace DailySpin.Website
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<UserAccount>, UserRepository>();
            services.AddScoped<IBaseRepository<BetsGlass>, BetGlassRepository>();
            services.AddScoped<IBaseRepository<Bet>, BetRepository>();
            services.AddScoped<IBaseRepository<Roulette>, RouletteRepository>();
            services.AddScoped<IBaseRepository<Chip>, ChipRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBetsGlassService, BetsGlassService>();
            services.AddScoped<IBetService, BetService>();
            services.AddScoped<IRouletteService, RouletteService>();
        }
    }
}