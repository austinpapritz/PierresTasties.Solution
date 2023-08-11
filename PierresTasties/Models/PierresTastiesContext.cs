using Microsoft.EntityFrameworkCore;

namespace PierresTasties.Models;

public class PierresTastiesContext : DbContext
{
    public DbSet<ExampleModel> ExampleModels { get; set; }
    public DbSet<Treat> Treats { get; set; }
    public DbSet<Flavor> Flavors { get; set; }
    public DbSet<FlavorTreat> FlavorTreats { get; set; }

    public PierresTastiesContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlavorTreat>()
            .HasKey(ft => new { ft.FlavorId, ft.TreatId });

    }

}
