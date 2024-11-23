using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ModelBase
    {
        public string Id { get; set; } 
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        //public Image Images { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
    }
}
