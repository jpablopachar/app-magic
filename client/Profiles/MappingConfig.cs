using AutoMapper;
using client.Dtos;

namespace client.Profiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<TownDto, TownRequestDto>().ReverseMap();
            CreateMap<TownDto, TownUpdateDto>().ReverseMap();

            CreateMap<TownNumberResponseDto, TownNumberRequestDto>().ReverseMap();
            CreateMap<TownNumberResponseDto, TownNumberUpdateDto>().ReverseMap();
        }
    }
}