using Microsoft.EntityFrameworkCore;
using Tesla;

class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<TeslaCar> TeslaCars => Set<TeslaCar>();
    public DbSet<CityCode> CityCodes => Set<CityCode>();
    public DbSet<Comment> Comment => Set<Comment>();
}
