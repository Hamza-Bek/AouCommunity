namespace Domain.Models;

public class Comment
{
  public string id { get; set; }
  public string content { get; set; }
  public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
  public string  UserId { get; set; }
  public string PostId { get; set; }
  
  public ApplicationUser User { get; set; }
  public Post Post { get; set; } 
}