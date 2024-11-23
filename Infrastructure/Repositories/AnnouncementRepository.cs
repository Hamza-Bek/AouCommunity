using Application.DTOs.Request.Entities;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly AppDbContext _context;

    public AnnouncementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralResponse> AddAnnouncementAsync(Announcement announcement, string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            
            if(user is null)
                return new GeneralResponse(false , "User does not exist");
            
            _context.Announcements.Add(announcement);
            
            await _context.SaveChangesAsync();
            
            return new GeneralResponse(true, "Announcement added successfully");
        }
        catch(Exception ex)
        {
            return new GeneralResponse(false, ex.Message);
        }
    }

    public async Task<GeneralResponse> UpdateAnnouncementAsync(Announcement announcement, string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);

            if (user is null)
                return new GeneralResponse(false, "User does not exist");

            var existingAnnouncement = await _context.Announcements.FindAsync(announcement.Id);
            
            if(existingAnnouncement is null)
                return new GeneralResponse(false , "Announcement does not exist");
            
            existingAnnouncement.Title = announcement.Title;
            existingAnnouncement.Content = announcement.Content;
            existingAnnouncement.Category = announcement.Category;
            
            _context.Announcements.Update(existingAnnouncement);
            await _context.SaveChangesAsync();
            
            return new GeneralResponse(true, "Announcement updated successfully");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(false, ex.Message);
        }
    }

    public async Task<GeneralResponse> RemoveAnnouncementAsync(string announcementId, string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
                return new GeneralResponse(false, "User does not exist");

            var announcement = await _context.Announcements.FindAsync(announcementId);
            if(announcement is null)
                return new GeneralResponse(false, "Announcement does not exist");

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();

            return new GeneralResponse(true, "Announcement removed successfully");

        }
        catch(Exception ex)
        {
            return new GeneralResponse(false, ex.Message);
        }
    }

    public async Task<Announcement> GetAnnouncementById(string announcementId)
    {
        try
        {
            if (!string.IsNullOrEmpty(announcementId))
            {
                var announcement = await _context.Announcements.FindAsync(announcementId);
                return announcement;
            }
            else
            {
                throw new ArgumentNullException(nameof(announcementId));
            }
        }
        catch(Exception ex)
        {
            throw new (ex.Message);
        }
    }

    public async Task<IEnumerable<Announcement>> GetAllAnnouncementsAsync()
    {
        try
        {
            var announcements = await _context.Announcements.ToListAsync();
            return announcements;
        }
        catch(Exception ex)
        {
            throw new (ex.Message);
        }
    }
}