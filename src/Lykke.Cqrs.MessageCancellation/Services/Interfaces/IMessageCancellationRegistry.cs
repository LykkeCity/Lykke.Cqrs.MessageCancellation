using System;

namespace Lykke.Cqrs.MessageCancellation.Services.Interfaces
{
    public interface IMessageCancellationRegistry
    {
        void RegisterTypeWithMessageId<T>(Func<T, string> operationIdAccessor);

        void RegisterTypeWithMessageId(Type type, IMessageIdGetter getter);

        string GetMessageIdOrDefault(object message);
    }
}
