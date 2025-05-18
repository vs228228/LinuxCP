using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinuxCP.Application.Interfaces
{
    public interface IOllamaService
    {
        Task<string> GetResponseAsync(string prompt);
    }
}
