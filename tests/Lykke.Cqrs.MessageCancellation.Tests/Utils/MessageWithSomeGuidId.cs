using System;

namespace Lykke.Cqrs.MessageCancellation.Tests.Utils
{
    internal class MessageWithSomeGuidId
    {
        public Guid MessageId { get; set; }
    }
}
