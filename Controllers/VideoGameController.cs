using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoGamesAPI.Models;
using VideoGamesAPI.ViewModel;

namespace VideoGamesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly IVideoGameService _videoGameService;
        private readonly IMapper _mapper;

        public VideoGameController(IVideoGameService videoGameService, IMapper mapper)
        {
            _videoGameService = videoGameService;
            _mapper = mapper;
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
                    Response.Headers.Add("pagination", response.Metadata["pagination"]);
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
        public async Task<IActionResult> Add(CreateVideoGameModel videoGameModel)
        {
            ResponseMessage response = await _videoGameService.AddVideoGame(_mapper.Map<VideoGame>(videoGameModel));
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
        public async Task<IActionResult> Update(VideoGameModel videoGameModel)
        {
            ResponseMessage response = await _videoGameService.UpdateVideoGame(_mapper.Map<VideoGame>(videoGameModel));
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
        public async Task<IActionResult> GetFiltered([FromQuery] int?[] arrGenreId, [FromQuery] PageParameters pageParameters)
        {
            ResponseMessage response = await _videoGameService.GetVideoGamesFiltered(arrGenreId, pageParameters);
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
    }
}
