using System;

namespace Lykke.Cqrs.MessageCancellation.Services.Interfaces
{
    public interface IMessageIdGetter
    {
        string GetMessageId(object objectWithMessageId);
    }
}
