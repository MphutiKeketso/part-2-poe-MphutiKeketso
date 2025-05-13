using Microsoft.EntityFrameworkCore;
using EventEaseBookingSystem.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Venue> Venue { get; set; }
    public DbSet<Event> Event { get; set; }
    public DbSet<Booking> Booking { get; set; }

  /*  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("tcp:cldv-eventeasebookingsystem.database.windows.net,1433;Initial Catalog=EventEaseBookingSystemDB;Persist Security Info=False;User ID=NehaSingh;Password=Misty@16;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30",
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
    }*/
}
