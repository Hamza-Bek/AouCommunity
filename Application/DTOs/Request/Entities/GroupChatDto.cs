using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Entities
{
    public class GroupChatDto
    {
        public int Id { get; set; }

        [Required]
        public string? SenderId { get; set; }

        [Required]
        public string? SenderName { get; set; }

        [Required]
        public string? Message { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
