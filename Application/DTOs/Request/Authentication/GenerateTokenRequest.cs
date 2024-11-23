using System.Text.Json;

namespace Application.DTOs.Request.AuthenticationDto;

public class GenerateTokenRequest
{
    public string Id { get; set; }                // User ID
    public string UserName { get; set; }          // Username
    public string Email { get; set; }             // User email
    public string Role { get; set; }              // User role
    public string FullName { get; set; }          // Full name 
    public Dictionary<string, JsonElement> Claims { get; set; }
}