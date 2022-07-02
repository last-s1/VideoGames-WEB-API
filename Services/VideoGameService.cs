using VideoGamesAPI.Models;
using System.Text.Json;

namespace VideoGamesAPI.Services
{
    /// <summary>
    /// Класс реализующий логику взаимодействия с сущностью базы данных VideoGame
    /// </summary>
    public class VideoGameService : IVideoGameService
    {
        private DataContext _dataContext;
        private ILogger _logger;
        private JsonSerializerOptions _jsonOption = new JsonSerializerOptions();
        public VideoGameService(DataContext context, ILogger logger)
        {
            _dataContext = context;
            _logger = logger;
            _jsonOption.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        /// <summary>
        /// Получить полный список видео игр
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseMessage> GetVideoGameList(PageParameters pageParameters)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var videoGames = await _dataContext.VideoGames
                                                        .Include(v => v.Genres)
                                                        .ToListAsync();

                var pagedVideoGames = PagedList<VideoGame>.ToPagedList(videoGames, pageParameters.PageNumber, pageParameters.PageSize);

                response.StatusCode = 200;
                response.Message = "Список видео игр успешно получен";
                response.Content = JsonSerializer.Serialize(pagedVideoGames, _jsonOption);
                response.Metadata = pagedVideoGames.ReturnPaginationMetaData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось получить список видео игр";
            }

            return response;
        }

        public async Task<ResponseMessage> GetVideoGame(int id)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var videoGame = await _dataContext.VideoGames
                                        .Where(v => v.Id == id)
                                        .Include(v => v.Genres)
                                        .FirstOrDefaultAsync();
                if (videoGame != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Данные видео игры успешно получены";
                    response.Content = JsonSerializer.Serialize(videoGame, _jsonOption);
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Ошибка: видео игра не найдена";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось получить данные видео игры";
            }

            return response;
        }

        public async Task<ResponseMessage> AddVideoGame(VideoGame videoGame)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                _dataContext.Database.BeginTransaction();

                bool success = true;
                VideoGame newVideoGame = new VideoGame { Title = videoGame.Title, Developers = videoGame.Developers };
                // Insert game
                await _dataContext.VideoGames.AddAsync(newVideoGame);
                await _dataContext.SaveChangesAsync();

                var existingGenres = await _dataContext.Genres.AsNoTracking().ToListAsync();
                // Insert game-genre
                foreach (var genreModel in videoGame.Genres)
                {
                    if (existingGenres.Any(g => g.Id == genreModel.Id && g.Name == genreModel.Name))
                        newVideoGame.Genres.Add(genreModel);
                    else
                    {
                        success = false;
                        response.StatusCode = 404;
                        response.Message = $"Ошибка: попытка добавить несуществующий жанр (Id : {genreModel.Id}, Name: {genreModel.Name})";
                        _dataContext.Database.RollbackTransaction();
                        break;
                    }
                }
                if (success)
                {
                    await _dataContext.SaveChangesAsync();
                    _dataContext.Database.CommitTransaction();

                    response.StatusCode = 201;
                    response.Message = "Видео игра успешно добавлена";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось добавить видео игру";
                _dataContext.Database.RollbackTransaction();
            }

            return response;
        }

        public async Task<ResponseMessage> UpdateVideoGame(VideoGame requestVideoGame)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                bool success = true;

                var videoGame = await _dataContext.VideoGames
                                        .Where(v => v.Id == requestVideoGame.Id)
                                        .Include(g => g.Genres)
                                        .FirstOrDefaultAsync();

                if (videoGame != null)
                {
                    // Update game
                    _dataContext.Entry(videoGame).CurrentValues.SetValues(requestVideoGame);

                    // Delete genre
                    foreach (var existingGenre in videoGame.Genres)
                    {
                        if (!requestVideoGame.Genres.Any(g => g.Id == existingGenre.Id))
                            videoGame.Genres.Remove(existingGenre);
                    }

                    var existingGenres = await _dataContext.Genres.AsNoTracking().ToListAsync();
                    // Insert game-genre
                    foreach (var genreModel in requestVideoGame.Genres)
                    {
                        var existingGenre = videoGame.Genres
                            .Where(v => v.Id == genreModel.Id && v.Name == genreModel.Name)
                            .SingleOrDefault();

                        if (existingGenre == null)
                        {
                            if (existingGenres.Any(g => g.Id == genreModel.Id && g.Name == genreModel.Name))
                                videoGame.Genres.Add(genreModel);
                            else
                            {
                                success = false;
                                response.StatusCode = 404;
                                response.Message = $"Ошибка: попытка добавить несуществующий жанр (Id : {genreModel.Id}, Name: {genreModel.Name})";
                                break;
                            }

                        }
                    }
                }
                else
                {
                    success = false;
                    response.StatusCode = 404;
                    response.Message = $"Ошибка: игра с заданным идентификатором не найдена (id: {requestVideoGame.Id})";
                }

                if (success)
                { 
                    await _dataContext.SaveChangesAsync();
                    response.StatusCode = 200;
                    response.Message = "Видео игра успешно обновлена";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось обновить видео игру";
            }

            return response;
        }

        public async Task<ResponseMessage> DeleteVideoGame(int id)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var videoGame = await _dataContext.VideoGames.FindAsync(id);
                if (videoGame != null)
                {
                    _dataContext.VideoGames.Remove(videoGame);
                    await _dataContext.SaveChangesAsync();

                    response.StatusCode = 200;
                    response.Message = "Игра успешно удалена";
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = $"Ошибка: игра с заданным идентификатором не найдена (id: {id})";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось удалить видео игру";
            }
            return response;
        }

        /// <summary>
        /// Получить отфильтрованный список видео игр
        /// </summary>
        /// <param name="arrGenreId">Идентификатор жанра</param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetVideoGamesFiltered(int?[] arrGenreId, PageParameters pageParameters)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                if (arrGenreId.Count() > 0)
                {
                    List<VideoGame> videoGames = await _dataContext.VideoGames
                                                        .Include(v => v.Genres)
                                                        .Where(v => v.Genres.Any(g => arrGenreId.Contains(g.Id)))
                                                        .ToListAsync();
                    var pagedVideoGames = PagedList<VideoGame>.ToPagedList(videoGames, pageParameters.PageNumber, pageParameters.PageSize);

                    response.StatusCode = 200;
                    response.Message = $"Список видео игр отфильтрованный по жанру успешно получен (idGenre: {arrGenreId})";
                    response.Content = JsonSerializer.Serialize(pagedVideoGames, _jsonOption);
                    response.Metadata = pagedVideoGames.ReturnPaginationMetaData();
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = $"Ошибка: в запрос не передано ни одного индентификатора жанра";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось получить отфильтрованный список видео игр";
            }

            return response;
        }
    }
}
