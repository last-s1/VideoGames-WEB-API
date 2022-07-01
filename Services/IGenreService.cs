using VideoGamesAPI.Models;

namespace VideoGamesAPI.Services
{
    public interface IGenreService
    {
        public Task<ResponseMessage> GetGenreList(PageParameters pageParameters);
        public Task<ResponseMessage> AddGenre(Genre genre);
        public Task<ResponseMessage> UpdateGenre(Genre requestGenre);
        public Task<ResponseMessage> DeleteGenre(int id);

    }
}
