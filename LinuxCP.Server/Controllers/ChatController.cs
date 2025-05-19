using LinuxCP.Application.Interfaces;
using LinuxCP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LinuxCP.Server.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private readonly IOllamaService _ollamaService;

        public ChatController(IOllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        [HttpGet("Messages/{userId}")]
        public Task <IActionResult> GetAllMessageByUserAsync(int userId)
        {
            return Task.FromResult<IActionResult>(NotFound("Пользователь не найден или метод не реализован"));
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
