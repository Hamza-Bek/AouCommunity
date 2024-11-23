using System.Text.Json;

namespace Application.DTOs.Request.AuthenticationDto;

public class GenerateTokenRequest
{
    public string Email { get; set; }
    public Guid Id { get; set; }   
    public Dictionary<string, JsonElement> Claims { get; set; }
}