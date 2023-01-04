using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.DataProvider.Repository;
using DailySpin.Logic.Interfaces;
using DailySpin.Logic.Services;

namespace DailySpin.Website
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<UserAccount>, UserRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}