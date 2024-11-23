

namespace Domain.Models
{
    public class Announcement : ModelBase
    { 
        public string Category { get; set; } = string.Empty;
        public DateTime LastUpdatedDate { get; set; }
    }
}
