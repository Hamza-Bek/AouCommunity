using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Entities
{
    public class IndividualChatDto
    {
        public string? SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
