using VideoGamesAPI.Models;

namespace VideoGamesAPI.Services
{
    public interface IVideoGameService
    {
        public Task<ResponseMessage> GetVideoGameList();
        public Task<ResponseMessage> GetVideoGame(int id);
        public Task<ResponseMessage> GetVideoGamesFiltered(int? genreId);
        public Task<ResponseMessage> AddVideoGame(VideoGame genre);
        public Task<ResponseMessage> UpdateVideoGame(VideoGame requestGenre);
        public Task<ResponseMessage> DeleteVideoGame(int id);
    }
}
