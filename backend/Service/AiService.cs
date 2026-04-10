using backend.Entity.DTO;
using System.Text;
using System.Text.Json;

namespace backend.Service
{
    public class AiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["AIApi:ApiKey"];
        }

        public async Task<string> GenerateDescription(DeviceRequest device)
        {
            var prompt = $@"
                Generate a short, professional description for this device:

                Name: {device.Name}
                Manufacturer: {device.Manufacturer}
                Type: {device.DeviceType}
                OS: {device.OS} {device.OSVersion}
                Processor: {device.Processor}
                RAM: {device.RamAmount} GB

                Keep it under 2 sentences.";

            var requestBody = new
            {
                model = "nvidia/nemotron-3-super-120b-a12b:free",
                messages = new[]
            {
                new { role = "user", content = prompt }
            },
                reasoning = new { enabled = true},
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var result = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return result ?? "No description generated.";
        }
    }
}
