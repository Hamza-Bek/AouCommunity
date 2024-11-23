using Application.DTOs.Request.Entities;
using Application.DTOs.Response;
using Domain.Models;

namespace Application.Interfaces;

public interface IEventRepository
{
    Task<GeneralResponse> AddEventAsync(Event model, string userId);
    Task<GeneralResponse> UpdateEventAsync(Event model, string userId);
    Task<GeneralResponse> RemoveEventAsync(string eventId , string userId);

    Task<Event> GetEventById(string eventId);
    Task<IEnumerable<Event>> GetAllEventsAsync();
}