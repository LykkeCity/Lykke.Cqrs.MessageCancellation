using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Cqrs.MessageCancellation.Interceptors;
using Lykke.Cqrs.MessageCancellation.Services;
using Lykke.Cqrs.MessageCancellation.Tests.Utils;
using Lykke.Cqrs.Middleware;
using Moq;
using Xunit;

namespace Lykke.Cqrs.MessageCancellation.Tests.Interceptors
{
    public class MessageCancellationInterceptorsTests
    {
        [Fact]
        public async Task CommandsInterceptor__CancellationRequested__FlowIsCancelled()
        {
            var command = PrepareServicesForInterception(out var operationId, 
                out var messageCancellationService, 
                out var messageCancellationRegistry,
                out var logFactory);

            var messageCancellationCommandInterceptor = 
                new MessageCancellationCommandInterceptor(
                    messageCancellationService,
                    messageCancellationRegistry, 
                    logFactory.Object);
            var interceptionContext = new Mock<ICommandInterceptionContext>();
            interceptionContext.Setup(x => x.Command).Returns(command);
            interceptionContext.Setup(x => x.HandlerObject).Returns(this);
            interceptionContext.Setup(x => x.InvokeNextAsync())
                .Returns(Task.FromResult(CommandHandlingResult.Fail(TimeSpan.Zero)));
            var result = await messageCancellationCommandInterceptor.InterceptAsync(interceptionContext.Object);

            Assert.False(result.Retry);
        }

        [Fact]
        public async Task EventsInterceptor__CancellationRequested__FlowIsCancelled()
        {
            var @event = PrepareServicesForInterception(out var operationId,
                out var messageCancellationService,
                out var messageCancellationRegistry,
                out var logFactory);

            var messageCancellationCommandInterceptor =
                new MessageCancellationEventInterceptor(
                    messageCancellationService,
                    messageCancellationRegistry,
                    logFactory.Object);
            var interceptionContext = new Mock<IEventInterceptionContext>();
            interceptionContext.Setup(x => x.Event).Returns(@event);
            interceptionContext.Setup(x => x.HandlerObject).Returns(this);
            interceptionContext.Setup(x => x.InvokeNextAsync())
                .Returns(Task.FromResult(CommandHandlingResult.Fail(TimeSpan.Zero)));
            var result = await messageCancellationCommandInterceptor.InterceptAsync(interceptionContext.Object);

            Assert.False(result.Retry);
        }

        [Fact]
        public async Task CommandsInterceptor__CancellationIsNotRequested__FlowIsCompleted()
        {
            var command = PrepareServicesForInterception(out var operationId,
                out var messageCancellationService,
                out var messageCancellationRegistry,
                out var logFactory);
            await messageCancellationService.RemoveMessageFromCancellationAsync(operationId);

            var messageCancellationCommandInterceptor =
                new MessageCancellationCommandInterceptor(
                    messageCancellationService,
                    messageCancellationRegistry,
                    logFactory.Object);
            var interceptionContext = new Mock<ICommandInterceptionContext>();
            interceptionContext.Setup(x => x.Command).Returns(command);
            interceptionContext.Setup(x => x.HandlerObject).Returns(this);
            interceptionContext.Setup(x => x.InvokeNextAsync())
                .Returns(Task.FromResult(CommandHandlingResult.Fail(TimeSpan.Zero)));
            var result = await messageCancellationCommandInterceptor.InterceptAsync(interceptionContext.Object);

            Assert.True(result.Retry);
        }

        [Fact]
        public async Task EventsInterceptor__CancellationIsNotRequested__FlowIsCompleted()
        {
            var @event = PrepareServicesForInterception(out var operationId,
                out var messageCancellationService,
                out var messageCancellationRegistry,
                out var logFactory);
            await messageCancellationService.RemoveMessageFromCancellationAsync(operationId);

            var messageCancellationCommandInterceptor =
                new MessageCancellationEventInterceptor(
                    messageCancellationService,
                    messageCancellationRegistry,
                    logFactory.Object);
            var interceptionContext = new Mock<IEventInterceptionContext>();
            interceptionContext.Setup(x => x.Event).Returns(@event);
            interceptionContext.Setup(x => x.HandlerObject).Returns(this);
            interceptionContext.Setup(x => x.InvokeNextAsync())
                .Returns(Task.FromResult(CommandHandlingResult.Fail(TimeSpan.Zero)));
            var result = await messageCancellationCommandInterceptor.InterceptAsync(interceptionContext.Object);

            Assert.True(result.Retry);
        }

        private static MessageWithSomeId PrepareServicesForInterception(out string operationId,
            out MessageCancellationService messageCancellationService,
            out MessageCancellationRegistry messageCancellationRegistry, out Mock<ILogFactory> logFactory)
        {
            operationId = Guid.NewGuid().ToString();
            var command = new MessageWithSomeId()
            {
                MessageId = operationId
            };
            messageCancellationService = new MessageCancellationService();
            messageCancellationRegistry = new MessageCancellationRegistry();
            messageCancellationRegistry.RegisterTypeWithMessageId<MessageWithSomeId>(x => x.MessageId.ToString());
            messageCancellationService.RequestMessageCancellationAsync(operationId).Wait();
            logFactory = new Mock<ILogFactory>();
            logFactory.Setup(x => x.CreateLog(It.IsAny<object>())).Returns(new Mock<ILog>().Object);
            return command;
        }
    }
}

