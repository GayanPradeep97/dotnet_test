using GameStore.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Game> games { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new { Id = 1, Name = "Fighting" },
                new { Id = 2, Name = "Fighting" },
                new { Id = 3, Name = "Sports" },
                new { Id = 4, Name = "Racing" },
                new { Id = 5, Name = "Kids and Family" }
            );
        }
    }



}
