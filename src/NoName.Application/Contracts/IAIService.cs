using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace NoName.Application.Contracts
{
    public interface IAIService
    {

        Task<string> GetAIResponseAsync(string prompt, string userRole = "Guest", CancellationToken ct = default);
        // Task<string> GetAIResponseAsync(string prompt, CancellationToken ct = default);
    }
}
