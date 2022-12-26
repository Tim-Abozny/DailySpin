using DailySpin.Website.Enums;
using DailySpin.Website.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DailySpin.Website.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Chip> Chips { get; set; }
    public DbSet<WinChipHistory> WinHistory { get; set; }
    public DbSet<BetsGlass> BetsGlasses { get; set; }
    public DbSet<Bet> Bets { get; set; }
    //public DbSet<UserAccount> Users { get; set; } = null!;
  
    
    
    /*using (AppDbContext db = new AppDbContext())
{
   UserAccount antonio = new UserAccount { Name = "Antonio", Email = "banderos@gmail.com", Password = "zorro_1", Balance = 0, Image = "C:\\Users\\progr\\source\\repos\\C#\\5sem\\trainee\\root\\Source\\DailySpin.Website\\wwwroot\\img\\avatarImg.jpg" };
   db.Users.Add(antonio);
   db.SaveChanges();
}
*/
}
