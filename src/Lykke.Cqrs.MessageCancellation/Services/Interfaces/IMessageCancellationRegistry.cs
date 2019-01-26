using System;

namespace Lykke.Cqrs.MessageCancellation.Services.Interfaces
{
    public interface IMessageCancellationRegistry
    {
        void RegistryTypeWithMessageId<T>(Func<T, Guid> operationIdAccessor);

        Guid? GetMessageId(object objectWithOperationId);
    }
}
