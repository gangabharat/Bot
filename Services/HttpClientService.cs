using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Bot.Services
{
    public interface IHttpClientService
    {
        string? ClientName { get; set; }
        void Init(string? clientName);
        public Task<HttpResponseMessage> Send(ApiType ApiType, string Url, string? AccessToken = null, object? Payload = null);
    }
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClient;

        public string? ClientName { get; set; } = "bot";
        //public string? BaseAddress { get; set; }
        public HttpClientService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public void Init(string? clientName)
        {
            ClientName = clientName;
        }

        public async Task<HttpResponseMessage> Send(ApiType ApiType, string Url, string? AccessToken = null, object? Payload = null)
        {
            if (ClientName is null)
            {
                throw new Exception();
            }

            var client = _httpClient.CreateClient(ClientName);

            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(Url);
            client.DefaultRequestHeaders.Clear();
            if (Payload != null)
            {
                message.Content = new StringContent(
                    JsonSerializer.Serialize(Payload, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }),
                    Encoding.UTF8, "application/json");

                Console.WriteLine("Request Payload {0}", await message.Content.ReadAsStringAsync());
                //message.Content = new StringContent(Payload, Encoding.UTF8, "application/json");
            }

            if (!string.IsNullOrEmpty(AccessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            }

            switch (ApiType)
            {
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }
            return await client.SendAsync(message);
        }
    }
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
