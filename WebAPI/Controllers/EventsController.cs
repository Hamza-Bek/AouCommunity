using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventRepository _eventRepository;
    private readonly IValidator<EventDto> _eventValidator;
    
    public EventsController(IEventRepository eventRepository, IValidator<EventDto> eventValidator)
    {
        _eventRepository = eventRepository;
        _eventValidator = eventValidator;
    }

    [HttpPost("create/event")]
    public async Task<IActionResult> AddEventAsync(EventDto model, string userId)
    {
        var validationResult = await _eventValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var eventModel = new Event()
        {
            Id = Guid.NewGuid().ToString(),
            Title = model.Title,
            Content = model.Content,
            Author = userId,
            Category = model.Category,
            CreatedDate = DateTime.UtcNow,
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };
        
        var result = await _eventRepository.AddEventAsync(eventModel, userId);
        return Ok(result);
    }

    [HttpPut("update/event")]
    public async Task<IActionResult> UpdateEventAsync(EventDto model, string userId)
    {
        var validationResult = await _eventValidator.ValidateAsync(model);
        
        if(!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var eventModel = new Event()
        {
            Id = Guid.NewGuid().ToString(),
            Title = model.Title,
            Content = model.Content,
            Author = userId,
            Category = model.Category,
            CreatedDate = DateTime.UtcNow,
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };
        
        var result = await _eventRepository.UpdateEventAsync(eventModel, userId);
        
        return Ok(result); 
    }

    [HttpDelete("delete/event")]
    public async Task<IActionResult> DeleteEventAsync(string eventId, string userId)
    {
        var result = await _eventRepository.RemoveEventAsync(eventId, userId);
        return Ok(result);
    }

    [HttpGet("get/event/{eventId}")]
    public async Task<IActionResult> GetEventById(string eventId)
    {
        var result = await _eventRepository.GetEventById(eventId);
        return Ok(result);
    }

    [HttpGet("get/events")]
    public async Task<IActionResult> GetAllEventsAsync()
    {
        var result = await _eventRepository.GetAllEventsAsync();
        return Ok(result);
    }
}