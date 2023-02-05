using DailySpin.DataProvider.Models;
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
    public DbSet<Roulette> Roulettes { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}