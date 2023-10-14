using static utility.DS;

namespace client.Models
{
    public class ApiRequest
    {
        public API_TYPE ApiType { get; set; } = API_TYPE.GET;
        public string? Url { get; set; }
        public object? Data { get; set; }
        public string? Token { get; set; }
        public Parameters? Parameters { get; set; }
    }

    public class Parameters {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}