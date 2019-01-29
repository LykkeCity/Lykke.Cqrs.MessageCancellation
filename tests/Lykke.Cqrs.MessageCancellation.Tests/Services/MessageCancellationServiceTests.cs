using System;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Cqrs.MessageCancellation.Services;
using Xunit;

namespace Lykke.Cqrs.MessageCancellation.Tests.Services
{
    public class MessageCancellationServiceTests
    {
        [Fact]
        public async Task CheckFlow()
        {
            var operationId = Guid.NewGuid().ToString();
            var messageCancellationService = new MessageCancellationService();
            var firstCheck = await messageCancellationService.CheckIfOperationRequiresCancellationAsync(operationId);
            var firstCheckEnumerable = await messageCancellationService.GetAllMessagesToCancellAsync();
            await messageCancellationService.RequestMessageCancellationAsync(operationId);
            var secondCheck = await messageCancellationService.CheckIfOperationRequiresCancellationAsync(operationId);
            var secondCheckEnumerable = await messageCancellationService.GetAllMessagesToCancellAsync();
            await messageCancellationService.RemoveMessageFromCancellationAsync(operationId);
            var thirdCheck = await messageCancellationService.CheckIfOperationRequiresCancellationAsync(operationId);
            var thirdCheckEnumerable = await messageCancellationService.GetAllMessagesToCancellAsync();

            Assert.False(firstCheck);
            Assert.True(secondCheck);
            Assert.False(thirdCheck);

            Assert.True(firstCheckEnumerable.Count() == 0);
            Assert.True(secondCheckEnumerable.Count() == 1);
            Assert.True(secondCheckEnumerable.FirstOrDefault() == operationId);
            Assert.True(thirdCheckEnumerable.Count() == 0);
        }

    }
}
