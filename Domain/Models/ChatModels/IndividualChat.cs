using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ChatModels
{
    public class IndividualChat
    {
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
    }
}
