using client.Models;

namespace client.Services
{
    public interface IBaseService
    {
        public ApiResponse apiResponse { get; set; }
        public Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}