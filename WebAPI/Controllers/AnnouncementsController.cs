using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IValidator<AnnouncementDto> _announcementValidator;

    public AnnouncementsController(IAnnouncementRepository announcementRepository, IValidator<AnnouncementDto> announcementValidator)
    {
        _announcementRepository = announcementRepository;
        _announcementValidator = announcementValidator;
    }

    [HttpPost("create/announcement")]
    public async Task<IActionResult> AddAnnouncementAsync([FromBody] AnnouncementDto model, string userId)
    {
        var validationResult = await _announcementValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var announcementModel = new Announcement()
        {
            Id = Guid.NewGuid().ToString(),
            Title = model.Title,
            Content = model.Content,
            Author = userId,
            CreatedDate = DateTime.UtcNow,
            Category = model.Category,
        };
        
        var result = await _announcementRepository.AddAnnouncementAsync(announcementModel, userId);
        return Ok(result);
    }

    [HttpPut("update/announcement")]
    public async Task<IActionResult> UpdateAnnouncementAsync([FromBody] AnnouncementDto model, string userId)
    {
        var validationResult = await _announcementValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var announcementModel = new Announcement()
        {
            Id = model.Id,
            Title = model.Title,
            Content = model.Content,
            Author = userId,
            CreatedDate = DateTime.UtcNow,
            Category = model.Category,
        };
        
        var result = await _announcementRepository.UpdateAnnouncementAsync(announcementModel, userId);
        
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("remove/announcement")]
    public async Task<IActionResult> DeleteAnnouncementAsync(string announcementId, string userId)
    {
        var result = await _announcementRepository.RemoveAnnouncementAsync(announcementId, userId);
        return Ok();
    }

    [HttpGet("get/announcement/{announcementId}")]
    public async Task<IActionResult> GetAnnouncementById(string announcementId)
    {
        var data = await _announcementRepository.GetAnnouncementById(announcementId);
        return Ok(data);
    }

    [Authorize]
    [HttpGet("get/announcements")]
    public async Task<IActionResult> GetAnnouncements()
    {
        var data = await _announcementRepository.GetAllAnnouncementsAsync();
        return Ok(data);
    }
    
}