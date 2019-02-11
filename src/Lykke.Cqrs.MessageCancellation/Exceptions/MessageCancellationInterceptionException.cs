using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Cqrs.MessageCancellation.Exceptions
{
    public class MessageCancellationInterceptionException : Exception
    {
        public MessageCancellationInterceptionException(string message) : base(message)
        {
        }
    }
}
