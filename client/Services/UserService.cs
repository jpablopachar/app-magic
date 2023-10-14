using client.Dtos;
using client.Models;
using static utility.DS;

namespace client.Services
{
    public class UserService : BaseService, IUserService
    {
        public readonly IHttpClientFactory _httpClient;

        private readonly string _townUrl;

        public UserService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _townUrl = configuration.GetValue<string>("ServiceUrls:API_URL")!;
        }

        public Task<T> Login<T>(LoginRequestDto loginRequestDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.POST,
                Data = loginRequestDto,
                Url = $"{_townUrl}/api/v1/user/login"
            });
        }

        public Task<T> Register<T>(RegisterRequestDto registerRequestDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.POST,
                Data = registerRequestDto,
                Url = $"{_townUrl}/api/v1/user/register"
            });
        }
    }
}