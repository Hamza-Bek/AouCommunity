using Application.DTOs.Request.Entities;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface IEmailRepository
{
    Task<GeneralResponse> SendEmailAsync(MailRequest mailRequest, string userId);
}