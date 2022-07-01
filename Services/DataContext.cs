using VideoGamesAPI.Models;

namespace VideoGamesAPI.Services
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<VideoGame> VideoGames { get; set; } = null!;
    }
}
