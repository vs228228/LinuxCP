using LinuxCP.Application.Interfaces;
using LinuxCP.Domain.Entities;
using LinuxCP.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

public class ChatService : IChatService
{
    private readonly IOllamaService _ollamaService;
    private readonly PostgresDbContext _context;

    public ChatService(IOllamaService ollamaService, PostgresDbContext context)
    {
        _ollamaService = ollamaService;
        _context = context;
    }

    public async Task<IEnumerable<ChatMessage>> GetAllMessagesByUserAsync(int userId)
    {
        return await _context.СhatMessages
            .Where(m => m.UserId == userId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<string> SendMessageAsync(string prompt, int userId)
    {
        return await _ollamaService.GetResponseAsync(prompt, userId);
    }

    public async Task ClearHistoryAsync(int userId)
    {
        var messages = _context.СhatMessages.Where(m => m.UserId == userId);
        _context.СhatMessages.RemoveRange(messages);
        await _context.SaveChangesAsync();
    }
}
