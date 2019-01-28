using System;
using Lykke.Cqrs.MessageCancellation.Services;
using Lykke.Cqrs.MessageCancellation.Tests.Utils;
using Xunit;

namespace Lykke.Cqrs.MessageCancellation.Tests.Services
{
    public class MessageCancellationRegistryTests
    {
        [Fact]
        public void GetMessageId__MessageTypeIsRegistered__IdReceived()
        {
            var objectWithMessageId = new MessageWithSomeId()
            {
                MessageId = Guid.NewGuid().ToString()
            };
            var messageCancellationRegistry = new MessageCancellationRegistry();
            //Example of a message registration
            messageCancellationRegistry.RegistryTypeWithMessageId<MessageWithSomeId>((x) => x.MessageId);
            var messageId = messageCancellationRegistry.GetMessageIdOrDefault(objectWithMessageId);
            
            Assert.Equal(objectWithMessageId.MessageId, messageId);
        }

        [Fact]
        public void GetMessageId__MessageTypeIsNotRegistered__IdIsNull()
        {
            var objectWithMessageId = new MessageWithSomeId()
            {
                MessageId = Guid.NewGuid().ToString()
            };
            var messageCancellationRegistry = new MessageCancellationRegistry();
            var messageId = messageCancellationRegistry.GetMessageIdOrDefault(objectWithMessageId);

            Assert.Null(messageId);
        }

    }
}
