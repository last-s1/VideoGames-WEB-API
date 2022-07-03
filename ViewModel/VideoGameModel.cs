namespace VideoGamesAPI.ViewModel
{
    public class VideoGameModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Developers { get; set; }
        public ICollection<GenreModel> Genres { get; set; }
    }
}
