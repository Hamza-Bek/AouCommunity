using Domain.Enums;
using Domain.Models;

namespace Application.DTOs.Request.Entities;

public class PostDto
{
    public string Id { get; set; } 
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; }
    public EntityStatus Status { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
}