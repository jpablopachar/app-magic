using client.Dtos;

namespace client.Services
{
    public interface IUserService
    {
        public Task<T> Login<T>(LoginRequestDto loginRequestDto);

        public Task<T> Register<T>(RegisterRequestDto registerRequestDto);
    }
}