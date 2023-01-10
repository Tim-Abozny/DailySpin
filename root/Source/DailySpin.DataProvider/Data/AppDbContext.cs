using DailySpin.Website.Models;
using Microsoft.EntityFrameworkCore;

namespace DailySpin.DataProvider.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Chip> Chips { get; set; }
    public DbSet<WinChipHistory> WinHistory { get; set; }
    public DbSet<BetsGlass> BetsGlasses { get; set; }
    public DbSet<Bet> Bets { get; set; }
    public DbSet<UserAccount> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      /*byte[] imageArray = File.ReadAllBytes("C:\\Users\\progr\\source\\repos\\C#\\5sem\\trainee\\root\\Source\\DailySpin.Website\\wwwroot\\img\\blueChip.png");*/
        
        base.OnModelCreating(builder);
/*        builder.Entity<BetsGlass>().HasData(
            new BetsGlass
            {
                Id = new Guid("C2EE299A-8AC2-4B91-8D12-1FD07C68B086"),
                BetMultiply = 2,
                BetsCount = 1,
                Bets = new List<Bet>(),
                TotalBetSum = 0,
                GlassImage = imageArray
            });*/
    }
}