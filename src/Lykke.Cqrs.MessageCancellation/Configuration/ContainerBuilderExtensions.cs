﻿using System;
using Autofac;
using Lykke.Cqrs.MessageCancellation.Interceptors;
using Lykke.Cqrs.MessageCancellation.Services;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Lykke.Cqrs.MessageCancellation.Configuration
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCqrsMessageCancellation(
            this ContainerBuilder containerBuilder, 
            Action<IOptions<RegisterCommandOption>> registerCommandsAction)
        {
            //Interceptors
            containerBuilder.RegisterType<MessageCancellationCommandInterceptor>();
            containerBuilder.RegisterType<MessageCancellationEventInterceptor>();

            containerBuilder.RegisterType<MessageCancellationRegistry>()
                .As<IMessageCancellationRegistry>()
                .IfNotRegistered(typeof(IMessageCancellationRegistry))
                .SingleInstance();

            containerBuilder.RegisterType<MessageCancellationService>()
                .As<IMessageCancellationService>()
                .IfNotRegistered(typeof(IMessageCancellationService))
                .SingleInstance();

            containerBuilder.RegisterBuildCallback((container) =>
            {
                var registry = container.Resolve<IMessageCancellationRegistry>();

                var registrationOption = new RegisterCommandOption();
                var options = Options.Create<RegisterCommandOption>(registrationOption);
                registerCommandsAction(options);
                var registered = registrationOption.GetRegisteredFunctions();

                foreach (var dictItem in registered)
                {
                    registry.RegisterTypeWithMessageId(dictItem.Key, dictItem.Value);
                }
            });

            return containerBuilder;
        }
    }
}
