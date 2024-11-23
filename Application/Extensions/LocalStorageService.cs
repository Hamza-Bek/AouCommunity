using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTOs.Request.Account;
using Application.Extensions;
using Blazored.LocalStorage;

namespace Application.Extension
{
    public class LocalStorageService(ILocalStorageService localStorageService)
    {

        private async Task<string> GetBrowserLocalStoarge()
        {
            var tokenModel = await localStorageService.GetItemAsStringAsync(Constants.BrowserStorageKey);
            return tokenModel!;
        }

        public async Task<LocalStorageDto> GetModelFromToken()
        {
            try
            {
                string token = await GetBrowserLocalStoarge();
                if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
                    return new LocalStorageDto();

                return DeserializeJsonString<LocalStorageDto>(token);

            }
            catch
            {
                return new LocalStorageDto();
            }
        }
        public async Task SetBrowserLocalStorage(LocalStorageDto localStorageDTO)
        {
            try
            {
                string token = SerializeObj(localStorageDTO);
                await localStorageService.SetItemAsStringAsync(Constants.BrowserStorageKey, token);
            }
            catch { }
        }

        public async Task RemoveTokenFromBrowserLocalStorage() => await localStorageService.RemoveItemAsync(Constants.BrowserStorageKey);

        private static string SerializeObj<T>(T modelObject) => JsonSerializer.Serialize(modelObject, JsonOptions());
        private static T DeserializeJsonString<T>(string jsonString)
            => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;

        private static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
            };
        }
        
        public async Task<string> GetUserIdFromToken()
        {
            var tokenModel = await GetModelFromToken();
            if (string.IsNullOrWhiteSpace(tokenModel?.Token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenModel.Token);

            // Extract the "sub" or "userId" claim, depending on your JWT structure
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "userId")?.Value;

            return userId;
        }
    }
}