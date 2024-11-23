using Domain.Enums;

namespace Application.DTOs.Request.Entities;

public class OfferDto
{
    public string Id { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    //public Image Images { get; set; }
    public decimal? Price { get; set; }       
    public EntityStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}