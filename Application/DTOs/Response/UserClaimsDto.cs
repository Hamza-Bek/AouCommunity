namespace Application.DTOs.Response.Account
{
    public record UserClaimsDto(
        string Id = null!,
        string FullName = null!,
        string UserName = null!,
        string Email = null!,
        string Role = null!,
        bool EmailConfirmed = false);
}