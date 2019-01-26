using Autofac;
using JetBrains.Annotations;
using Lykke.Cqrs.MessageCancellation.Services;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;

namespace Lykke.Cqrs.MessageCancellation.Modules
{
    [UsedImplicitly]
    public class MessageCancellationModule : Module
    {
        public MessageCancellationModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageCancellationRegistry>()
                .As<IMessageCancellationRegistry>()
                .SingleInstance();

            builder.RegisterType<MessageCancellationService>()
                .As<IMessageCancellationService>()
                .SingleInstance();
        }
    }
}
