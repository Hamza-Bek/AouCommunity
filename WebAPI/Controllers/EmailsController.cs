using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailsController : ControllerBase
{
    private readonly IEmailRepository _emailRepository;

    public EmailsController(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
    }

    [HttpPost("send/email")]
    public async Task<IActionResult> SendEmail(MailRequest request, string userId)
    {
        try
        {
            await _emailRepository.SendEmailAsync(request, userId);
            return Ok();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    
    
}