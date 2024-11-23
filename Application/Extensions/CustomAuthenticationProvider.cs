using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.DTOs.Request.Account;
using Application.DTOs.Response.Account;
using Application.Extension;
using Microsoft.AspNetCore.Components.Authorization;

namespace Application.Extensions;

public class CustomAuthenticationProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        try
        {
            var tokenModel = await localStorageService.GetModelFromToken();

            if (string.IsNullOrWhiteSpace(tokenModel.Token))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserClaims = DecryptToken(tokenModel.Token!);
            if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var claimsPrincipal = SetClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(anonymous));
        }
    }
    
    public async Task UpdateAuthenticationState(LocalStorageDto localStorageDTO)
    {
        ClaimsPrincipal claimsPrincipal = anonymous;
        if (localStorageDTO.Token != null)
        {
            await localStorageService.SetBrowserLocalStorage(localStorageDTO);
            Console.WriteLine("Token saved to local storage");

            var getUserClaims = DecryptToken(localStorageDTO.Token!);
            if (getUserClaims != null)
            {
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
                Console.WriteLine("ClaimsPrincipal updated");
            }
            else
            {
                Console.WriteLine("Token decryption failed");
            }
        }
        else
        {
            await localStorageService.RemoveTokenFromBrowserLocalStorage();
            Console.WriteLine("Token removed from local storage");
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public static ClaimsPrincipal SetClaimPrincipal(UserClaimsDto claims)
    {
        if (claims.Email is null) return new ClaimsPrincipal();
        return new ClaimsPrincipal(new ClaimsIdentity(
        [
            new(ClaimTypes.NameIdentifier, claims.Id),
            new(ClaimTypes.Name , claims.UserName!),
            new(ClaimTypes.Email , claims.Email!),
            new(ClaimTypes.Role, claims.Role),
            new Claim ("Fullname", claims.FullName),
        ], Constants.AuthenticationType));
    }

    private static UserClaimsDto DecryptToken(string jwtToken)
    {
        try
        {
            if (string.IsNullOrEmpty(jwtToken)) return new UserClaimsDto();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var name = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;
            var fullname = token.Claims.FirstOrDefault(c => c.Type == "Fullname")?.Value ?? string.Empty;
            var id = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var emailConfirmed = token.Claims.FirstOrDefault(c => c.Type == "EmailConfirmed")?.Value;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(email))
            {
                // Log the missing claims
                Console.WriteLine("Missing required claims in the token");
                return null;
            }

            bool isEmailConfirmed = bool.TryParse(emailConfirmed, out bool result) && result;

            return new UserClaimsDto(id, fullname, name, email, role, isEmailConfirmed);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token decryption failed: {ex.Message}");
            return null;
        }
    }
}