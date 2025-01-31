using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ChatModels
{
    public class Thread
    {
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Message { get; set; }
        public ConnectionRequestStatus Status{ get; set; } = ConnectionRequestStatus.Pending;
        public DateTime Date { get; set; }
    }
}
