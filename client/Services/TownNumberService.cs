using client.Dtos;
using client.Models;
using static utility.DS;

namespace client.Services
{
    public class TownNumberService : BaseService, ITownNumberService
    {
        public readonly IHttpClientFactory _httpClient;

        private readonly string _townNumberUrl;

        public TownNumberService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _townNumberUrl = configuration.GetValue<string>("ServiceUrls:API_URL")!;
        }

        public Task<T> CreateTownNumber<T>(string token, TownNumberRequestDto townNumberRequestDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.POST,
                Data = townNumberRequestDto,
                Url = $"{_townNumberUrl}/api/v1/townNumber",
                Token = token
            });
        }

        public Task<T> GetTownNumber<T>(string token, int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.GET,
                Url = $"{_townNumberUrl}/api/v1/townNumber/{id}",
                Token = token
            });
        }

        public Task<T> GetTownNumbers<T>(string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.GET,
                Url = $"{_townNumberUrl}/api/v1/townNumber",
                Token = token
            });
        }

        public Task<T> RemoveTownNumber<T>(string token, int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.DELETE,
                Url = $"{_townNumberUrl}/api/v1/townNumber/{id}",
                Token = token
            });
        }

        public Task<T> UpdateTownNumber<T>(string token, TownNumberUpdateDto townNumberUpdateDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = API_TYPE.PUT,
                Data = townNumberUpdateDto,
                Url = $"{_townNumberUrl}/api/v1/townNumber/{townNumberUpdateDto.TownNo}",
                Token = token
            });
        }
    }
}