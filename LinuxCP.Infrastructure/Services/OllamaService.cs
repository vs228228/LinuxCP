using LinuxCP.Application.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LinuxCP.Infrastructure.Persistence.Contexts;
using LinuxCP.Domain.Entities;  // не забудь using

namespace LinuxCP.Infrastructure.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _ollamaUrl;
        private readonly PostgresDbContext _postgresDbContext;

        public OllamaService(HttpClient httpClient, IConfiguration configuration, PostgresDbContext postgresDbContext)
        {
            _httpClient = httpClient;
            _ollamaUrl = configuration["OllamaApiUrl"];
            _postgresDbContext = postgresDbContext;
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

            // Создаем сущности сообщений
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

            // Добавляем сообщения в контекст и сохраняем
            _postgresDbContext.СhatMessages.Add(userMessage);
            _postgresDbContext.СhatMessages.Add(botMessage);
            await _postgresDbContext.SaveChangesAsync();

            return botResponse;
        }

    }
}
