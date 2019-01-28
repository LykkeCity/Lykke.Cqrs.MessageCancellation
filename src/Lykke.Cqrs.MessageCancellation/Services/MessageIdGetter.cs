using System;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;

namespace Lykke.Cqrs.MessageCancellation.Services
{
    internal class MessageIdGetter<T> : IMessageIdGetter
    {
        private readonly Func<T, string> _messageIdAccessor;

        public MessageIdGetter(Func<T, string> messageIdAccessor)
        {
            _messageIdAccessor = messageIdAccessor;
        }

        public string GetMessageId(object objectWithOperationId)
        {
            var obj = (T)objectWithOperationId;

            return _messageIdAccessor(obj);
        }
    }
}
