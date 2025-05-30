using LinuxCP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxCP.Infrastructure.Persistence.Contexts
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<ChatMessage> СhatMessages { get; set; }

    }
}
