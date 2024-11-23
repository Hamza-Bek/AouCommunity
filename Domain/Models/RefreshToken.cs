namespace Domain.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? Token { get; set; }
}