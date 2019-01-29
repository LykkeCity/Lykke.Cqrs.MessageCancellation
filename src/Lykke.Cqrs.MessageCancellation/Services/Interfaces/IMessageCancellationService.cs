using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Cqrs.MessageCancellation.Services.Interfaces
{
    public interface IMessageCancellationService
    {
        Task RequestMessageCancellationAsync(string messageId);

        Task RemoveMessageFromCancellationAsync(string messageId);

        Task<bool> CheckIfOperationRequiresCancellationAsync(string messageId);

        Task<IEnumerable<string>> GetAllMessagesToCancellAsync();
    }
}
