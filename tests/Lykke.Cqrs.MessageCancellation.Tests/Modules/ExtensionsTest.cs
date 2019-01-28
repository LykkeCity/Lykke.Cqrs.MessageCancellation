using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Lykke.Cqrs.MessageCancellation.Modules;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;
using Lykke.Cqrs.MessageCancellation.Tests.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Xunit;

namespace Lykke.Cqrs.MessageCancellation.Tests.Modules
{
    public class ExtensionsTest
    {
        [Fact]
        public void TestExtension()
        {
            var builder = new ContainerBuilder();

            builder.RegisterCqrsMessageCancellation((options) =>
                {
                    options.Value
                        .MapMessageId<MessageWithSomeId>(x => x.MessageId)
                        .MapMessageId<MessageWithGuidId>(x => x.OperationId.ToString());
                });

            var container = builder.Build();
            var registry = container.Resolve<IMessageCancellationRegistry>();
            var guid = Guid.NewGuid();

            var result1 = registry.GetMessageId(new MessageWithSomeId() {MessageId = "1"});
            var result2 = registry.GetMessageId(new MessageWithGuidId() { OperationId = guid });

            Assert.Equal("1", result1);
            Assert.Equal(guid.ToString(), result2);
        }

        internal class MessageWithGuidId
        {
            public Guid OperationId { get; set; }
        }
    }
}
