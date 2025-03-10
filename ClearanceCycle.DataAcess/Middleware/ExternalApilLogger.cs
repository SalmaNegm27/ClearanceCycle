

namespace ClearanceCycle.DataAcess.Middleware
{
    public class ExternalApilLogger
    {
        private readonly HttpClient _client;

        private readonly ILogger<ExternalApilLogger> _logger;


        public ExternalApilLogger(ILogger<ExternalApilLogger> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }
        public async Task<T> SendAsync<T>(string url, HttpMethod method, object? body = null, string? token = null)
        {
            var request = new HttpRequestMessage(method, url);
            if (body != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(body),Encoding.UTF8, "application/json");
            }

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", $"Bearer {token}");
            }
            //request.Content.Headers.Add("Content-Type", "application/json");

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var logObject = new
            {
                Request = new
                {
                    URL = request.RequestUri,
                    Method = request.Method,
                    Headers = request.Headers, 
                },
                Time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff"),
                Response = new
                {
                    StatusCode = response.StatusCode,
                    Headers = response.Headers,
                    Content = content ?? "No content",
                }
            };
            _logger.LogInformation("{ExternalApi}", logObject);
            return JsonConvert.DeserializeObject<T>(content);
        }


    }
}

