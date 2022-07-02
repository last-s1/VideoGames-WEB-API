using VideoGamesAPI.Models;
using System.Text.Json;

namespace VideoGamesAPI.Services
{
    /// <summary>
    /// Класс реализующий логику взаимодействия с сущностью базы данных Genre
    /// </summary>
    public class GenreService : IGenreService
    {
        private DataContext _dataContext;
        private ILogger _logger;
        private JsonSerializerOptions _jsonOption = new JsonSerializerOptions();
        public GenreService(DataContext context, ILogger logger)
        {
            _dataContext = context;
            _logger = logger;
            _jsonOption.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        /// <summary>
        /// Получить полный список жанров
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseMessage> GetGenreList(PageParameters pageParameters)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                List<Genre> genres = await _dataContext.Genres.ToListAsync();
                var pagedGenres = PagedList<Genre>.ToPagedList(genres, pageParameters.PageNumber, pageParameters.PageSize);

                response.StatusCode = 200;
                response.Message = "Список видео игр успешно получен";
                response.Content = JsonSerializer.Serialize(pagedGenres, _jsonOption);
                response.Metadata = pagedGenres.ReturnPaginationMetaData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось получить список жанров";
            }

            return response;
        }

        public async Task<ResponseMessage> AddGenre(Genre genre)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                await _dataContext.Genres.AddAsync(genre);
                await _dataContext.SaveChangesAsync();

                response.StatusCode = 201;
                response.Message = "Жанр успешно добавлен";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось добавить жанр";
            }

            return response;
        }
        public async Task<ResponseMessage> UpdateGenre(Genre requestGenre)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                bool success = true;

                var genre = await _dataContext.Genres
                                    .Where(g => g.Id == requestGenre.Id)
                                    .FirstOrDefaultAsync();

                if(genre != null)
                {
                    _dataContext.Entry(genre).CurrentValues.SetValues(requestGenre);
                }
                else
                {
                    success = false;
                    response.StatusCode = 404;
                    response.Message = $"Ошибка: жанр с заданным идентификатором не найден (id: {requestGenre.Id})";
                }
                
                if (success)
                {
                    await _dataContext.SaveChangesAsync();
                    response.StatusCode = 200;
                    response.Message = "Жанр успешно обновлен";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось обновить жанр";
            }

            return response;
        }

        public async Task<ResponseMessage> DeleteGenre(int id)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var genre = await _dataContext.Genres.FindAsync(id);
                if (genre != null)
                {
                    _dataContext.Genres.Remove(genre);
                    await _dataContext.SaveChangesAsync();

                    response.StatusCode = 200;
                    response.Message = "Жанр успешно удален";
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = $"Ошибка: жанр с заданным идентификатором не найден (id: {id})";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                response.StatusCode = 500;
                response.Message = "Ошибка: не удалось удалить жанр";
            }

            return response;
        }
    }
}
