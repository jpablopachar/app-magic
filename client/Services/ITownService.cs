using client.Dtos;

namespace client.Services
{
    public interface ITownService
    {
        public Task<T> GetTowns<T>(string token);

        public Task<T> GetTownsPaged<T>(string token, int pageNumber = 1, int pageSize = 4);

        public Task<T> GetTown<T>(string token, int id);

        public Task<T> CreateTown<T>(string token, TownRequestDto townRequestDto);

        public Task<T> UpdateTown<T>(string token, TownUpdateDto townUpdateDto);

        public Task<T> RemoveTown<T>(string token, int id);
    }
}