using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HotelBookingApi.Models
{
    public class AIAgentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AIAgentService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> AskAgent(string context, string question)
        {
            var apiKey = _config["OpenAI:ApiKey"];

            var requestData = new
            {
                model = "gpt-4", // لو عندك مجاني ممكن gpt-3.5-turbo
                messages = new[]
                {
                new { role = "system", content = $"Use this context to answer: {context}" },
                new { role = "user", content = question }
            }
            };

            var requestJson = JsonConvert.SerializeObject(requestData);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            requestMessage.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            var responseJson = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseJson)!;
            return result?.choices?[0]?.message?.content ?? "No response from AI.";
        }
    }
}
