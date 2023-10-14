using client.Dtos;

namespace client.Services
{
    public interface ITownNumberService
    {
        public Task<T> GetTownNumbers<T>(string token);

        public Task<T> GetTownNumber<T>(string token, int id);

        public Task<T> CreateTownNumber<T>(string token, TownNumberRequestDto townNumberRequestDto);

        public Task<T> UpdateTownNumber<T>(string token, int id, TownNumberUpdateDto townNumberUpdateDto);

        public Task<T> RemoveTownNumber<T>(string token, int id);
    }
}