using Microsoft.AspNetCore.Mvc;
using VideoGamesAPI.Models;

namespace VideoGamesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> Get([FromQuery] PageParameters pageParameters)
        {
            ResponseMessage response = await _genreService.GetGenreList(pageParameters);
            switch (response.StatusCode)
            {
                case >= 400:
                    return Problem(
                        statusCode: response.StatusCode,
                        title: response.Message);
                default:
                    Response.Headers.Add("Pagination", response.Metadata);
                    return Ok(response.Content);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Genre genre)
        {
            ResponseMessage response = await _genreService.AddGenre(genre);
            switch (response.StatusCode)
            {
                case >= 400:
                    return Problem(
                        statusCode: response.StatusCode,
                        title: response.Message);
                default:
                    return StatusCode(201, response.Content);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Genre requestGenre)
        {
            ResponseMessage response = await _genreService.UpdateGenre(requestGenre);
            switch (response.StatusCode)
            {
                case >= 400:
                    return Problem(
                        statusCode: response.StatusCode,
                        title: response.Message);
                default:
                    return Ok(response.Content);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            ResponseMessage response = await _genreService.DeleteGenre(id);
            switch (response.StatusCode)
            {
                case >= 400:
                    return Problem(
                        statusCode: response.StatusCode,
                        title: response.Message);
                default:
                    return Ok(response.Message);
            }
        }

    }
}
