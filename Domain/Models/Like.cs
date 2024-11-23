namespace Domain.Models;

public class Like
{
    public string Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int Count { get; set; }
}