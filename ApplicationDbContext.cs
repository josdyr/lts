using Microsoft.EntityFrameworkCore;

class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Object> Objects => Set<Object>();
}
