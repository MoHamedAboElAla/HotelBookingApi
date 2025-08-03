using HotelBookingApi.Data;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly AIAgentService _aiService;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AIController(AIAgentService aiService, IConfiguration config , HttpClient httpClient)
        {
            _aiService = aiService;
            _config = config;
            _httpClient = httpClient;
        }
        [HttpPost("ask")]
        public async Task<string> AskAgent([FromBody] string question)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "AIContext", "Hotel.txt");
            var contextText = await System.IO.File.ReadAllTextAsync(filePath);

            var apiKey = _config["OpenRouter:ApiKey"];
            var requestData = new
            {
                model = "deepseek/deepseek-chat-v3-0324:free",
                messages = new[]
                {
             new { role = "system", content = $"جاوب فقط بناءً على المعلومات الموجودة في هذا النص، ولا تضف أي استنتاجات: {contextText}" },
            new { role = "user", content = question }
        }
            };

            var requestJson = JsonConvert.SerializeObject(requestData);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            requestMessage.Headers.Add("HTTP-Referer", "https://yourdomain.com");
            requestMessage.Headers.Add("X-Title", "HotelBookingAI");
            requestMessage.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            var responseJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseJson);

            dynamic result = JsonConvert.DeserializeObject(responseJson)!;
            string? answer = result?.choices?[0]?.message?.content;

            if (string.IsNullOrWhiteSpace(answer))
                return $"No response from AI.\nFull Response:\n{responseJson}";

            answer = answer.Trim();

            if (answer.StartsWith("إذًا") || answer.StartsWith("لذلك") || answer.Contains("الإجابة هي"))
                answer = answer.Replace("الإجابة هي:", "").Replace("إذًا", "").Trim();

            return $"🔎 الإجابة:\n\n{answer}";
        }



    }
}
