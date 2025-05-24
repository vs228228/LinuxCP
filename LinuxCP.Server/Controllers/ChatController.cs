using LinuxCP.Application.Interfaces;
using LinuxCP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LinuxCP.Server.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("Messages/{userId}")]
        public async Task<IActionResult> GetAllMessageByUserAsync(int userId)
        {
            var messages = await _chatService.GetAllMessagesByUserAsync(userId);
            return Ok(messages);
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendMessageAsync([FromBody] OllamaRequest request, [FromQuery] int userId)
        {
            var response = await _chatService.SendMessageAsync(request.Prompt, userId);
            return Ok(new { response });
        }

        [HttpDelete("Clear/{userId}")]
        public async Task<IActionResult> ClearHistoryUserAsync(int userId)
        {
            await _chatService.ClearHistoryAsync(userId);
            return Ok($"История сообщений пользователя {userId} успешно очищена.");
        }
    }
}
