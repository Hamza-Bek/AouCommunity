using Application.DTOs.Request.Entities;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralResponse> AddEventAsync(Event model, string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
        
            if(user is null)
                return new GeneralResponse(false , "User does not exist");
            
            await _context.Events.AddAsync(model);
            
            await _context.SaveChangesAsync();
            
            return new GeneralResponse(true ,"Event made successfully");
        }
        catch(Exception ex)
        {
            return new GeneralResponse(false , "An error occured while adding new event!");
        }
        
    }

  
    public async Task<GeneralResponse> UpdateEventAsync(Event model, string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            
            if(user is null)
                return new GeneralResponse(false , "User does not exist");
            
            var existingEvent = await _context.Events.FindAsync(model.Id);

            if (existingEvent is null)
                return new GeneralResponse(false, "Event does not exist");

            existingEvent.Title = model.Title;
            existingEvent.Content = model.Content;
            existingEvent.Author = userId;  
            existingEvent.Category = model.Category;
            existingEvent.StartDate = model.StartDate;
            existingEvent.EndDate = model.EndDate;
            existingEvent.LastUpdatedDate = DateTime.UtcNow;
            
            _context.Events.Update(existingEvent);
            
            await _context.SaveChangesAsync();
            
            return new GeneralResponse(true , "Event updated successfully");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(false , ex.Message);
        }
    }

    public async Task<GeneralResponse> RemoveEventAsync(string eventId, string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            
            if(user is null)
                return new GeneralResponse(false , "User does not exist");
            
            var eventModel = await _context.Events.FirstOrDefaultAsync(i => i.Id == eventId);
            
            if(eventModel is null)
                return new GeneralResponse(false , "Event does not exist");

            _context.Events.Remove(eventModel);
            
            await _context.SaveChangesAsync();
            
            return new GeneralResponse(true, "Event removed successfully");

        }
        catch (Exception ex)
        {
            return new GeneralResponse(false, ex.Message);
        }
    }

    public async Task<Event> GetEventById(string eventId)
    {
        try
        {
            var eventModel = await _context.Events.FirstOrDefaultAsync(i => i.Id == eventId);
            return eventModel;
        }
        catch (Exception ex)
        {
            throw new (ex.Message);
        }
      
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        try
        {
            var events = await _context.Events.ToListAsync();
            return events;
        }
        catch (Exception ex)
        {
            throw new (ex.Message);
        }
    }
}