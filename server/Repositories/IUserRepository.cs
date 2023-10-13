using server.Dtos;

namespace server.Repositories
{
    public interface IUserRepository
    {
        bool IsUserUnique(string username);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        Task<UserDto> Register(RegisterRequestDto registerRequestDto);
    }
}