using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Lykke.Cqrs.MessageCancellation.Models
{
    [DataContract]
    public class MessageCancellationRequest
    {
        [Required]
        [DataMember(Name = "messageId")]
        public Guid MessageId { get; set; }
    }
}
