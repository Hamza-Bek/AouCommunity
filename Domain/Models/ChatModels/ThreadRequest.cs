using Domain.Enums;

namespace Domain.Models.ChatModels;

public class ThreadRequest
{ 
    public int Id { get; set; }
    public string? SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public ConnectionRequestStatus Status { get; set; } = ConnectionRequestStatus.Pending;
    public DateTime SentDate { get; set; } = DateTime.UtcNow;
}