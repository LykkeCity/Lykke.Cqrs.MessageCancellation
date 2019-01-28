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
            object contextMessage,
            object handlerObject,
            Func<Task<CommandHandlingResult>> funcAsync)
        {
            var messageId = messageCancellationRegistry.GetMessageId(contextMessage);
            if (string.IsNullOrEmpty(messageId))
                return await funcAsync();

            var requiresCancellation = await
                messageCancellationService.CheckIfOperationRequiresCancellationAsync(messageId);

            if (requiresCancellation)
            {
                log.Info($"MessageId: {messageId} is cancelled, " +
                         $"HandlerType: {handlerObject}, " +
                         $"MessageType: {contextMessage}", contextMessage);

                return Cqrs.CommandHandlingResult.Ok();
            }

            return await funcAsync();
        }
    }
}
