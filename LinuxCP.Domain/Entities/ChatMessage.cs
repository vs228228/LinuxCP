using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxCP.Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string Sender { get; set;} // "user" or "bot"
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
