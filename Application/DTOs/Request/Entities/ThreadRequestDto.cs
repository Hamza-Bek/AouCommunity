using Domain.Enums;

namespace Application.DTOs.Request.Entities;

public class ThreadRequestDto
{
    public int Id { get; set; }
    public string? SenderFullname { get; set; }
    public string? SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public ConnectionRequestStatus Status { get; set; } = ConnectionRequestStatus.Pending;
    public DateTime SentDate { get; set; } = DateTime.UtcNow;
}