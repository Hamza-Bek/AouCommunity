namespace Application.DTOs.Response.Account;

public record LoginResponse (bool Flag = false , string Email = null!, string Token = null!, string RefreshToken = null!);
