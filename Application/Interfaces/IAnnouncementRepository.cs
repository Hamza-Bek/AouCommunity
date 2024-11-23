using Application.DTOs.Request.Entities;
using Application.DTOs.Response;
using Domain.Models;

namespace Application.Interfaces;

public interface IAnnouncementRepository
{
    Task<GeneralResponse> AddAnnouncementAsync(Announcement announcement, string userId);
    Task<GeneralResponse> UpdateAnnouncementAsync(Announcement announcement, string userId);
    Task<GeneralResponse> RemoveAnnouncementAsync(string announcementId , string userId);
    
    Task<Announcement> GetAnnouncementById(string announcementId);
    Task<IEnumerable<Announcement>> GetAllAnnouncementsAsync();
}