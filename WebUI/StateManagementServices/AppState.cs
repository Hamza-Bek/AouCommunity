using Application.DTOs.Request.Entities;
using Domain.Models.ChatModels;

namespace WebUI.StateManagementServices;

public class AppState
{
    public List<ThreadRequestDto>? ThreadRequests { get; set; }
}