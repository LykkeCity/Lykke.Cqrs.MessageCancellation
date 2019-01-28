using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Cqrs.MessageCancellation.Models;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Cqrs.MessageCancellation.Controllers
{
    [PublicAPI, Route("/api/plugin/message-cancellation")]
    public class MessageCancellationController : Controller
    {
        private readonly IMessageCancellationService _messageCancellationService;

        public MessageCancellationController(IMessageCancellationService messageCancellationService)
        {
            _messageCancellationService = messageCancellationService ?? 
                                          throw new ArgumentException("Should not be null", 
                                              nameof(messageCancellationService));
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllMessagesAsync()
        {
            var allMessages = await _messageCancellationService.GetAllMessagesToCancellAsync();

            if (allMessages == null || !allMessages.Any())
                return NoContent();

            var response = new MessageCancellationManyResponse()
            {
                Messages = allMessages.Select(MessageId => new MessageCancellationResponse()
                {
                    MessageId = MessageId
                })
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddMessageForCancellationAsync(MessageCancellationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _messageCancellationService.RequestMessageCancellationAsync(request.MessageId);

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMessageFromCancellationAsync(MessageCancellationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _messageCancellationService.RemoveMessageFromCancellationAsync(request.MessageId);

            return Ok();
        }
    }
}
