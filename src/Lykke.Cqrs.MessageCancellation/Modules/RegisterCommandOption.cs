using Lykke.Cqrs.MessageCancellation.Services;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Lykke.Cqrs.MessageCancellation.Modules
{
    public class RegisterCommandOption
    {
        private readonly Dictionary<Type, IMessageIdGetter> _registeredTypes;

        public RegisterCommandOption()
        {
            _registeredTypes = new Dictionary<Type, IMessageIdGetter>();
        }

        public RegisterCommandOption MapMessageId<TMessage>(Func<TMessage, string> getMessageIdForTypeFunction)
        {
            _registeredTypes[typeof(TMessage)] = new MessageIdGetter<TMessage>(getMessageIdForTypeFunction);

            return this;
        }

        internal Dictionary<Type, IMessageIdGetter> GetRegisteredFunctions()
        {
            return _registeredTypes;
        }
    }
}
