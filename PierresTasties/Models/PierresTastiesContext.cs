using Microsoft.EntityFrameworkCore;

namespace PierresTasties.Models;

public class PierresTastiesContext : DbContext
{
    public DbSet<ExampleModel> ExampleModels { get; set; }

    public PierresTastiesContext(DbContextOptions options) : base(options) { }

}
