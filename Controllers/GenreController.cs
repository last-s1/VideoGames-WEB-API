using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoGamesAPI.Models;
using VideoGamesAPI.ViewModel;

namespace VideoGamesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;

        public GenreController(IGenreService genreService, IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
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
                    Response.Headers.Add("pagination", response.Metadata["pagination"]);
                    return Ok(response.Content);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateGenreModel genreModel)
        {
            ResponseMessage response = await _genreService.AddGenre(_mapper.Map<Genre>(genreModel));
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
        public async Task<IActionResult> Update(GenreModel genreModel)
        {
            ResponseMessage response = await _genreService.UpdateGenre(_mapper.Map<Genre>(genreModel));
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
