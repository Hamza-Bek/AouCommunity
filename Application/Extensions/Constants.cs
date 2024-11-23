namespace Application.Extensions;

public class Constants
{
    public const string RegisterRoute = "api/accounts/create";
    public const string LoginRoute = "api/accounts/login";
    public const string RefreshTokenRoute = "api/accounts/refresh-token";
  
    
    public const string AuthenticationType = "JwtAuth";
    public const string BrowserStorageKey = "x-key";
    public const string HttpClientName = "WebUI";
    public const string HttpClientHeaderScheme = "Bearer";
    
    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Seller = "Seller";
    }
}