using Microsoft.AspNetCore.Mvc;
using VideoGamesAPI.Models;

namespace VideoGamesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly IVideoGameService _videoGameService;

        public VideoGameController(IVideoGameService videoGameService)
        {
            _videoGameService = videoGameService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> Get([FromQuery] PageParameters pageParameters)
        {
            ResponseMessage response = await _videoGameService.GetVideoGameList(pageParameters);
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
      
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            ResponseMessage response = await _videoGameService.GetVideoGame(id);
            switch(response.StatusCode)
            {
                case >= 400:
                    return Problem(
                        statusCode: response.StatusCode,
                        title: response.Message);
                default:
                    return Ok(response.Content);
            }
                
        }

        [HttpPost]
        public async Task<IActionResult> Add(VideoGame videoGame)
        {
            ResponseMessage response = await _videoGameService.AddVideoGame(videoGame);
            switch (response.StatusCode)
            {
                case >= 400:
                    return Problem(
                        statusCode: response.StatusCode,
                        title: response.Message);
                default:
                    return StatusCode(201,response.Content);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(VideoGame requestVideoGame)
        {
            ResponseMessage response = await _videoGameService.UpdateVideoGame(requestVideoGame);
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
            ResponseMessage response = await _videoGameService.DeleteVideoGame(id);
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

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered(int? genreId, [FromQuery] PageParameters pageParameters)
        {
            ResponseMessage response = await _videoGameService.GetVideoGamesFiltered(genreId, pageParameters);
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
    }
}
