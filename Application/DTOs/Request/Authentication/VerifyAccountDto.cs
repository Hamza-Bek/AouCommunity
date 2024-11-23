namespace Application.DTOs.Request.Account;

public class VerifyAccountDto
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string VerificationCode { get; set; }
}