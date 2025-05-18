using LinuxCP.Application.Interfaces;
using LinuxCP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LinuxCP.Server.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class OllamaController : Controller
    {

        private readonly IOllamaService _ollamaService;
        public OllamaController(IOllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] OllamaRequest request)
        {
            var response = await _ollamaService.GetResponseAsync(request.Prompt);
            return Ok(new { response });
        }
    }
}
