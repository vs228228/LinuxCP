using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxCP.Domain.Entities
{
    public class User
    {
        public string Username { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
