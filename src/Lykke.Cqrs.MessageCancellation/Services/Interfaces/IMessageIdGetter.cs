using System;

namespace Lykke.Cqrs.MessageCancellation.Services.Interfaces
{
    internal interface IMessageIdGetter
    {
        Guid GetMessageId(object objectWithMessageId);
    }
}
