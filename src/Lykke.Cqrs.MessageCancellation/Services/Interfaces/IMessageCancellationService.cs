using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Cqrs.MessageCancellation.Services.Interfaces
{
    public interface IMessageCancellationService
    {
        Task RequestMessageCancellationAsync(Guid messageId);

        Task RemoveMessageFromCancellationAsync(Guid messageId);

        Task<bool> CheckIfOperationRequiresCancellationAsync(Guid messageId);

        Task<IEnumerable<Guid>> GetAllMessagesToCancellAsync();
    }
}
