using LinuxCP.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinuxCP.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly PostgresDbContext _postgresDbContext;

        public ExampleController(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _postgresDbContext.Users.ToListAsync();
            return Ok(users);
        }



    }
}
