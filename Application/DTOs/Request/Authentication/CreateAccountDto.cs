using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Account;

public class CreateAccountDto : LoginDto
{ 
    public string Name { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}