using LinuxCP.Application.Interfaces;
using LinuxCP.Domain.Entities;
using LinuxCP.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxCP.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly PostgresDbContext _context;

        public ChatRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task SaveChatMessagesAsync(ChatMessage userMessage, ChatMessage botMessage)
        {
            _context.СhatMessages.Add(userMessage);
            _context.СhatMessages.Add(botMessage);
            await _context.SaveChangesAsync();
        }
    }
}
