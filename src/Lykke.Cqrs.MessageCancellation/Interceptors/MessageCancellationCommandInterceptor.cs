using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Cqrs.Abstractions.Middleware;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;
using Lykke.Cqrs.Middleware;

namespace Lykke.Cqrs.MessageCancellation.Interceptors
{
    public class MessageCancellationCommandInterceptor: ICommandInterceptor
    {
        private readonly IMessageCancellationService _messageCancellationService;
        private readonly IMessageCancellationRegistry _messageCancellationRegistry;
        private readonly ILog _log;

        public MessageCancellationCommandInterceptor(IMessageCancellationService messageCancellationService,
            IMessageCancellationRegistry messageCancellationRegistry,
            ILogFactory loggerFactory)
        {
            _messageCancellationService = messageCancellationService;
            _messageCancellationRegistry = messageCancellationRegistry;
            _log = loggerFactory.CreateLog(this);
        }

        public async Task<CommandHandlingResult> InterceptAsync(ICommandInterceptionContext context)
        {
            return await CommonInterceptionLogic.InterceptAsync(
                _messageCancellationRegistry,
                _messageCancellationService,
                _log,
                context.Command, 
                context.HandlerObject, 
                context.InvokeNextAsync);
        }
    }
}
