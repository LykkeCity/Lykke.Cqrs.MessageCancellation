using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Lykke.Cqrs.MessageCancellation.Models
{
    [DataContract]
    public class MessageCancellationResponse
    {
        [Required]
        [DataMember(Name = "messageId")]
        public Guid MessageId { get; set; }
    }
}
