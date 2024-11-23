using System.Net.Http.Json;
using System.Reflection.Metadata;
using Application.DTOs.Request.Account;
using Application.DTOs.Request.AuthenticationDto;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Extensions;
using Application.Interfaces;
using Domain;
using Domain.Models;

namespace Application.Services;

public class AccountService : IAccountRepository
{
    private readonly HttpClientService _httpClientService;
    private readonly HttpClient _httpClient;

    public AccountService(HttpClientService httpClientService, HttpClient httpClient)
    {
        _httpClientService = httpClientService;
        _httpClient = httpClient;
    }

    public async Task<LoginResponse> LoginAccountAsync(LoginDto model)
    {
        try
        {
            var publicClient = _httpClientService.GetPublicClient();

            var response = await publicClient.PostAsJsonAsync(Constants.LoginRoute, model);

            string error = CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
                return new LoginResponse(false, error);

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result!;
        }
        catch(Exception ex)
        {
            return new LoginResponse(false, ex.Message);
        }
    }

    public async Task<GeneralResponse> CreateAccountAsync(CreateAccountDto model)
    {
        try
        {
            var publicClient = _httpClientService.GetPublicClient();
            
            var response = await publicClient.PostAsJsonAsync(Constants.RegisterRoute, model);

            string error = CheckResponseStatus(response);

            if(!string.IsNullOrEmpty(error))
                return new GeneralResponse(false, error);
            
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!;
        }
        catch(Exception ex)
        {
            return new GeneralResponse(false, ex.Message);
        }
    }

    public Task<GeneralResponse> CreateRoleAsync(CreateRoleDto model)
    {
        throw new NotImplementedException();
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto model)
    {
        try
        {
            var publicClient = _httpClientService.GetPublicClient();
            var response = await publicClient.PostAsJsonAsync(Constants.RefreshTokenRoute, model);
            string error = CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
                return new LoginResponse(false, error);

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result!;
        }
        catch (Exception ex) { return new LoginResponse(false, ex.Message); }
    }

    public Task<string> GenerateTokenAsync(UserClaimsDto model)
    {
        throw new NotImplementedException();
    }

    public async Task<GetUserDto?> GetUserAsync(string userId)
    {
        try
        {
            var privateClient = await _httpClientService.GetPrivateClient();

            var response = await privateClient.GetAsync($"api/accounts/get/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"HTTP Error : {response.StatusCode}, Details: {error}");
            }
            
            var result = await response.Content.ReadFromJsonAsync<GetUserDto>();
            return result!;
        }
        catch(Exception ex)
        {
            throw new Exception($"HTTP Error : {ex.Message}");
        }
    }

    public async Task<GeneralResponse> VerifyAccountAsync(VerifyAccountDto model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/accounts/verify/account", model);
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<GeneralResponse>();

            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new GeneralResponse(false, errorMessage);
            }
        }
        catch (Exception ex) 
        {
                return new GeneralResponse(false, ex.Message); 
        }
        
        
    }
    
    private static string CheckResponseStatus(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            return $"sorry unkown error occured.{Environment.NewLine} Error Description : {Environment.NewLine} Status Code : {response.StatusCode}{Environment.NewLine} Reason Phrase : {response.ReasonPhrase}";
        else
            return null!;
    }
    
}