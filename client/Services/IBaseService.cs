using client.Models;

namespace client.Services
{
    public interface IBaseService
    {
        public ApiResponse ApiResponse { get; set; }
        public Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}