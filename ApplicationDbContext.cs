using Microsoft.EntityFrameworkCore;

class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<TeslaCar> TeslaCars => Set<TeslaCar>();
}
