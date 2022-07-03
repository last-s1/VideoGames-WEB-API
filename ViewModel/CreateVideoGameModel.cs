namespace VideoGamesAPI.ViewModel
{
    public class CreateVideoGameModel
    {
        public string Title { get; set; }
        public string Developers { get; set; }
        public ICollection<GenreModel> Genres { get; set; }
    }
}
