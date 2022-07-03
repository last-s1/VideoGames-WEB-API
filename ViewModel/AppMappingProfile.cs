using AutoMapper;
using VideoGamesAPI.Models;
using VideoGamesAPI.ViewModel;

namespace VideoGamesAPI.Services
{
	public class AppMappingProfile : Profile
	{
		public AppMappingProfile()
		{
			CreateMap<CreateVideoGameModel, VideoGame>();

			CreateMap<VideoGameModel, VideoGame>();

			CreateMap<CreateGenreModel, Genre>()
				.ForMember("Name", opt => opt.MapFrom(c => c.GenreName));

			CreateMap<GenreModel, Genre>()
				.ForMember("Name", opt => opt.MapFrom(c => c.GenreName));
		}
	}
}
