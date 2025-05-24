using LinuxCP.Application.Interfaces;
using LinuxCP.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinuxCP.Infrastructure.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _ollamaUrl;
        private readonly IChatRepository _chatRepository;

        public OllamaService(HttpClient httpClient, IConfiguration configuration, IChatRepository chatRepository)
        {
            _httpClient = httpClient;
            _ollamaUrl = configuration["OllamaApiUrl"];
            _chatRepository = chatRepository;
        }

        public async Task<string> GetResponseAsync(string prompt, int userId)
        {
            var requestBody = new
            {
                model = "deepseek-r1:1.5b",
                prompt,
                stream = false
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(_ollamaUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
                return $"Error: {response.StatusCode}";

            var responseBody = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseBody);
            var botResponse = doc.RootElement.GetProperty("response").GetString();

            var userMessage = new ChatMessage
            {
                UserId = userId,
                Sender = "user",
                Message = prompt,
                Timestamp = DateTime.UtcNow
            };

            var botMessage = new ChatMessage
            {
                UserId = userId,
                Sender = "bot",
                Message = botResponse,
                Timestamp = DateTime.UtcNow
            };

            await _chatRepository.SaveChatMessagesAsync(userMessage, botMessage);

            return botResponse;
        }
    }
}
