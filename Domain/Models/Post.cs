using Domain.Enums;

namespace Domain.Models;

public class Post
{
    public string Id { get; set; } 
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
    public ApplicationUser User { get; set; } 
    public EntityStatus Status { get; set; }
}