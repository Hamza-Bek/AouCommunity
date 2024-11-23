namespace Application.DTOs.Request.Entities;

public class CommentDto
{
    public string id { get; set; }
    public string Content { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; }
    public string PostId { get; set; }
}