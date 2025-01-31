using Application.DTOs.Request.Entities;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MimeKit;

namespace Infrastructure.Repositories;

public class EmailRepository : IEmailRepository
{
    private readonly MailSettings _mailSettings;
    private readonly AppDbContext _context;

    public EmailRepository(IOptions<MailSettings> mailSettings, AppDbContext context)
    {
        _mailSettings = mailSettings.Value;
        _context = context;
    }

    public async Task<GeneralResponse> SendEmailAsync(MailRequest mailRequest, string userId)
    {
        var user = await _context.Users.FindAsync(userId);

        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;
        var builder = new BodyBuilder();

        builder.HtmlBody = $" Your verification code is {user?.VerificationCode} , Please do not share it!";
        
        email.Body = builder.ToMessageBody();

        try
        {
          using var smtp = new MailKit.Net.Smtp.SmtpClient();
              smtp.Connect(_mailSettings.Host , _mailSettings.Port , SecureSocketOptions.StartTls);
                  smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                      await smtp.SendAsync(email);
                          smtp.Disconnect(true);
                          
            return new GeneralResponse(true, "Email sent!");
                          
        }
        catch(Exception ex)
        {
            return new GeneralResponse(false, ex.Message);
        }
    }
}