using System.Text.Json.Serialization;

namespace VideoGamesAPI.Models
{
    public class Genre
    {
        public Genre()
        {
            Games = new HashSet<VideoGame>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<VideoGame> Games { get; set; }
    }
}
