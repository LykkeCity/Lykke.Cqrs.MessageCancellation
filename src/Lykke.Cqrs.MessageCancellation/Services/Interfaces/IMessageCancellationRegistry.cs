using System;

namespace Lykke.Cqrs.MessageCancellation.Services.Interfaces
{
    public interface IMessageCancellationRegistry
    {
        void RegistryTypeWithMessageId<T>(Func<T, string> operationIdAccessor);

        void RegistryTypeWithMessageId(Type type, IMessageIdGetter getter);

        string GetMessageIdOrDefault(object message);
    }
}
