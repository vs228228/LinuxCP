using LinuxCP.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinuxCP.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly PostgresDbContext _postgresDbContext;

        public UserController(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }


    }
}
