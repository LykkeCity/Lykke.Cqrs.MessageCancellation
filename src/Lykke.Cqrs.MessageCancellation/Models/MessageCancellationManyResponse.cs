using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Lykke.Cqrs.MessageCancellation.Models
{
    [DataContract]
    public class MessageCancellationManyResponse
    {
        [Required]
        [DataMember(Name = "messages")]
        public IEnumerable<MessageCancellationResponse> Messages { get; set; }
    }
}
