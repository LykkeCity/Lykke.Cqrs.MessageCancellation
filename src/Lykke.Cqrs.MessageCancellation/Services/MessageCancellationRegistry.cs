﻿using System;
using System.Collections.Generic;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;

namespace Lykke.Cqrs.MessageCancellation.Services
{
    public class MessageCancellationRegistry : IMessageCancellationRegistry
    {
        private readonly Dictionary<Type, IMessageIdGetter> _registeredTypes;

        public MessageCancellationRegistry()
        {
            _registeredTypes = new Dictionary<Type, IMessageIdGetter>();
        }

        public void RegistryTypeWithMessageId<T>(Func<T, string> messageIdAccessor)
        {
            _registeredTypes[typeof(T)] = new MessageIdGetter<T>(messageIdAccessor);
        }

        public void RegistryTypeWithMessageId(Type type, IMessageIdGetter getter)
        {
            _registeredTypes[type] = getter;
        }

        public string GetMessageId(object objectWithOperationId)
        {
            var type = objectWithOperationId.GetType();

            if (!_registeredTypes.TryGetValue(type, out var operationIdGetter))
                return null;

            var operationId = operationIdGetter.GetMessageId(objectWithOperationId);

            return operationId;
        }
    }
}
