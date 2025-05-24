using LinuxCP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxCP.Application.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessage>> GetAllMessagesByUserAsync(int userId);
        Task<string> SendMessageAsync(string prompt, int userId);
        Task ClearHistoryAsync(int userId);
    }
}
