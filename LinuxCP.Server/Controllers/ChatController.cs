using LinuxCP.Application.Interfaces;
using LinuxCP.Domain.Entities;
using LinuxCP.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinuxCP.Server.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private readonly IOllamaService _ollamaService;
        private readonly PostgresDbContext _postgresDbContext;



        public ChatController(IOllamaService ollamaService, PostgresDbContext postgresDbContext)
        {
            _ollamaService = ollamaService;
            _postgresDbContext = postgresDbContext;
        }

        [HttpGet("Messages/{userId}")]
        public async Task <IActionResult> GetAllMessageByUserAsync(int userId)
        {
            var messages = await _postgresDbContext.СhatMessages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Ok(messages);
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendMessageAsync([FromBody] OllamaRequest request, int UserId)
        {
            var response = await _ollamaService.GetResponseAsync(request.Prompt, UserId);
            return Ok(new { response });
        }

        [HttpDelete("Clear/{userId}")]
        public async Task<IActionResult> ClearHistoryUserAsync(int userId)
        {
            return Ok($"История сообщений пользователя {userId} успешно очищена.");
        }


    }
}
