using System.Security.AccessControl;

namespace Application.DTOs.Request.Entities;

public class AnnouncementDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
}