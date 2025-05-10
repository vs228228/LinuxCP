using Microsoft.AspNetCore.Mvc;

namespace LinuxCP.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is working");
        }
    }
}
