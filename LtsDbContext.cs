using Microsoft.EntityFrameworkCore;

class LtsDbContext : DbContext
{
    public LtsDbContext(DbContextOptions<LtsDbContext> options)
        : base(options) { }

    public DbSet<Object> Objects => Set<Object>();
}
