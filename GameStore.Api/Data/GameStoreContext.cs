using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Title = "Fighting" },
            new { Id = 2, Title = "Roleplaying" },
            new { Id = 3, Title = "Sports" },
            new { Id = 4, Title = "Racing" },
            new { Id = 5, Title = "Kids and Family" }
        );
    }
}
