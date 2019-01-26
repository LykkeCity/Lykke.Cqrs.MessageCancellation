using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;

namespace Lykke.Cqrs.MessageCancellation.Interceptors
{
    public static class CommonInterceptionLogic
    {
        public static async Task<CommandHandlingResult> InterceptAsync(
            IMessageCancellationRegistry messageCancellationRegistry,
            IMessageCancellationService messageCancellationService,
            ILog log,
            object contextCommand,
            object handlerObject,
            Func<Task<CommandHandlingResult>> funcAsync)
        {
            var messageId = messageCancellationRegistry.GetMessageId(contextCommand);
            if (!messageId.HasValue)
                return await funcAsync();

            var requiresCancellation = await
                messageCancellationService.CheckIfOperationRequiresCancellationAsync(messageId.Value);

            if (requiresCancellation)
            {
                log.Info($"MessageId: {messageId.Value} is cancelled, " +
                         $"HandlerType: {handlerObject}, " +
                         $"MessageType: {contextCommand}", contextCommand);

                return Cqrs.CommandHandlingResult.Ok();
            }

            return await funcAsync();
        }
    }
}
