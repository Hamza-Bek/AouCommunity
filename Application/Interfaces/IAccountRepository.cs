using Application.DTOs.Request.Account;
using Application.DTOs.Request.AuthenticationDto;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;

namespace Application.Interfaces;

public interface IAccountRepository
{
    Task<LoginResponse> LoginAccountAsync(LoginDto model);
    Task<GeneralResponse> CreateAccountAsync(CreateAccountDto model);
    Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto model);
    Task<string> GenerateTokenAsync(UserClaimsDto model);
    Task<GetUserDto?> GetUserAsync(string userId);
    Task<GeneralResponse> VerifyAccountAsync(VerifyAccountDto model);
}