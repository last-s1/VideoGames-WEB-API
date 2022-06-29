namespace VideoGamesAPI.Models
{
    public class VideoGame
    {
        public VideoGame()
        {
            Genres = new HashSet<Genre>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Developers { get; set; } = null!;
        public virtual ICollection<Genre> Genres { get; set; }
    }
}
