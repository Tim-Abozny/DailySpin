using DailySpin.Website.Enums;
using DailySpin.Website.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailySpin.Website.Data;

public class AppDbContext : IdentityDbContext<UserAccount>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Chip> Chips { get; set; }
    public DbSet<WinChipHistory> WinHistory { get; set; }
    public DbSet<BetsGlass> BetsGlasses { get; set; }
    public DbSet<Bet> Bets { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserAccountEntityConfiguration());
    }
}

public class UserAccountEntityConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.Property(u => u.DisplayName).HasMaxLength(255);
        //builder.Property(u => u.Balance).HasDefaultValue(0);
        //builder.Property(u => u.Image).HasDefaultValue("C:\\Users\\progr\\source\\repos\\C#\\5sem\\trainee\\root\\Source\\DailySpin.Website\\wwwroot\\img\\avatarImg.jpg");
    }   
}
