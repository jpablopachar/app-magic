using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using client.Models;
using Newtonsoft.Json;
using static utility.DS;

namespace client.Services
{
    public class BaseService : IBaseService
    {
        public ApiResponse ApiResponse { get; set; }
        public IHttpClientFactory HttpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            HttpClient = httpClient;
            ApiResponse = new();
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = HttpClient.CreateClient("MagicAPI");

                HttpRequestMessage message = new HttpRequestMessage();

                message.Headers.Add("Accept", "application/json");

                if (apiRequest.Parameters == null)
                {
                    message.RequestUri = new(apiRequest.Url!);
                }
                else
                {
                    var builder = new UriBuilder(apiRequest.Url!);
                    var query = HttpUtility.ParseQueryString(builder.Query);

                    query["PageNumber"] = apiRequest.Parameters.PageNumber.ToString();
                    query["PageSize"] = apiRequest.Parameters.PageSize.ToString();

                    builder.Query = query.ToString();

                    string url = builder.ToString();

                    message.RequestUri = new(url);
                }

                if (apiRequest.Data != null) message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");

                message.Method = apiRequest.ApiType switch
                {
                    API_TYPE.POST => HttpMethod.Post,
                    API_TYPE.PUT => HttpMethod.Put,
                    API_TYPE.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get,
                };

                HttpResponseMessage apiResponse = null!;

                if (!string.IsNullOrEmpty(apiRequest.Token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                try
                {
                    ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(apiContent)!;

                    if (response != null && (apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Success = false;

                        var res = JsonConvert.SerializeObject(response);
                        var obj = JsonConvert.DeserializeObject<T>(res);

                        return obj!;
                    }
                }
                catch (Exception)
                {
                    var errorResponse = JsonConvert.DeserializeObject<T>(apiContent);

                    return errorResponse!;
                }

                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);

                return apiResponseDto!;
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    Success = false
                };

                var res = JsonConvert.SerializeObject(apiResponse);
                var resEx = JsonConvert.DeserializeObject<T>(res);

                return resEx!;
            }
        }
    }
}