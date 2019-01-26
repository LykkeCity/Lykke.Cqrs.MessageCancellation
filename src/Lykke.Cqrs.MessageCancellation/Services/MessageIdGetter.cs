using System;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;

namespace Lykke.Cqrs.MessageCancellation.Services
{
    internal class MessageIdGetter<T> : IMessageIdGetter
    {
        private readonly Func<T, Guid> _messageIdAccessor;

        public MessageIdGetter(Func<T, Guid> messageIdAccessor)
        {
            _messageIdAccessor = messageIdAccessor;
        }

        public Guid GetMessageId(object objectWithOperationId)
        {
            var obj = (T)objectWithOperationId;

            return _messageIdAccessor(obj);
        }
    }
}
