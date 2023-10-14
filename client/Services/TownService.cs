using client.Dtos;
using client.Models;
using static utility.DS;

namespace client.Services
{
    public class TownService : BaseService, ITownService
    {
        public readonly IHttpClientFactory _httpClient;

        private readonly string _townUrl;

        public TownService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _townUrl = configuration.GetValue<string>("ServiceUrls:API_URL")!;
        }

        public Task<T> CreateTown<T>(string token, TownRequestDto townRequestDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.POST,
                Data = townRequestDto,
                Url = $"{_townUrl}/api/v1/town",
                Token = token
            });
        }

        public Task<T> GetTown<T>(string token, int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.GET,
                Url = $"{_townUrl}/api/v1/town/{id}",
                Token = token
            });
        }

        public Task<T> GetTowns<T>(string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.GET,
                Url = $"{_townUrl}/api/v1/town",
                Token = token
            });
        }

        public Task<T> GetTownsPaged<T>(string token, int pageNumber = 1, int pageSize = 4)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.GET,
                Url = $"{_townUrl}/api/v1/town/townsPaged",
                Token = token,
                Parameters = new Parameters
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            });
        }

        public Task<T> RemoveTown<T>(string token, int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.DELETE,
                Url = $"{_townUrl}/api/v1/town/{id}",
                Token = token
            });
        }

        public Task<T> UpdateTown<T>(string token, TownUpdateDto townUpdateDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.PUT,
                Data = townUpdateDto,
                Url = $"{_townUrl}/api/v1/town/{townUpdateDto.Id}",
                Token = token
            });
        }
    }
}