using AutoMapper;
using server.Dtos;
using server.Models;

namespace server.Profiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Town, TownDto>();
            CreateMap<TownDto, Town>();

            CreateMap<Town, TownRequestDto>().ReverseMap();
            CreateMap<Town, TownUpdateDto>().ReverseMap();

            CreateMap<TownNumber, TownNumberResponseDto>().ReverseMap();
            CreateMap<TownNumber, TownRequestDto>().ReverseMap();
            CreateMap<TownNumber, TownUpdateDto>().ReverseMap();
            CreateMap<UserApp, UserDto>().ReverseMap();
        }
    }
}