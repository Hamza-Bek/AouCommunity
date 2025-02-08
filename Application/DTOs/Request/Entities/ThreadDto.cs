using Domain.Models.ChatModels;

namespace Application.DTOs.Request.Entities;

public class ThreadDto
{
    public string? ThreadId { get; set; }
    public string? User1ConnectionId { get; set; }
    public string? User2ConnectionId { get; set; }
    public string? User1Id { get; set; }
    public string? User2Id { get; set; }
}