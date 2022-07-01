using VideoGamesAPI.Models;

namespace VideoGamesAPI.Services
{
    public interface IVideoGameService
    {
        public Task<ResponseMessage> GetVideoGameList(PageParameters pageParameters);
        public Task<ResponseMessage> GetVideoGame(int id);
        public Task<ResponseMessage> GetVideoGamesFiltered(int?[] arrGenreId, PageParameters pageParameters);
        public Task<ResponseMessage> AddVideoGame(VideoGame genre);
        public Task<ResponseMessage> UpdateVideoGame(VideoGame requestGenre);
        public Task<ResponseMessage> DeleteVideoGame(int id);
    }
}
