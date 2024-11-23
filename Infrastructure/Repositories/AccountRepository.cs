using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Application.DTOs.Request.Account;
using Application.DTOs.Request.AuthenticationDto;
using Application.DTOs.Request.Entities;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
   
    private const string TokenSecret = "nM8mgxRKlkusaRsPzauqBsHDf1LmoYBukQ6JY5XaA_4=";
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(8);
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailRepository _emailRepository;
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private static readonly Random _random = new Random();
    public AccountRepository
    (
        UserManager<ApplicationUser> userManager
        ,RoleManager<IdentityRole> roleManager
        ,SignInManager<ApplicationUser> signInManager
        ,AppDbContext context
        ,IConfiguration config, IEmailRepository emailRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _context = context;
        _config = config;
        _emailRepository = emailRepository;
    }
    
    
    public async Task<LoginResponse> LoginAccountAsync(LoginDto model)
    {
        try
        {
            var user = await FindUserByEmailAsync(model.Email);

            if (user is null)
                return new LoginResponse(false, "Invalid email address");

            SignInResult result;
            try
            {
                result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            }
            catch
            {
                return new LoginResponse(false, "Invalid username or password");
            }

            if (!result.Succeeded)
                return new LoginResponse(false, "Invalid Credntials");

            var tokenRequest = new GenerateTokenRequest()
            {
                Email = user.Email,
                Id = Guid.Parse(user.Id),
                Claims = new Dictionary<string, JsonElement>
                {
                    { "Name", JsonDocument.Parse($"\"{user.Name}\"").RootElement }
                    // Add other claims here as needed
                }
            };
            
            string jwtToken = await GenerateTokenAsync(tokenRequest);
            string refreshToken = GenerateRefreshToken();

            if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrWhiteSpace(refreshToken))
                return new LoginResponse(false, "Error occurred while logging in, please try again");

            var saveResult = await SaveRefreshToken(user.Id, refreshToken);

            if (saveResult.Flag)
                return new LoginResponse(true, $"{user.Name} successfully logged in", jwtToken, refreshToken);
            else
                return new LoginResponse(false, "Error occurred while logging in, please try again");

        }
        catch (Exception ex)
        {
            return new LoginResponse(false, ex.Message);
        }
    }

    public async Task<GeneralResponse> CreateRoleAsync(CreateRoleDto model)
    {
        try
        {
            if ((await FindRoleByNameAsync(model.Name!)) == null)
            {
                var response = await _roleManager.CreateAsync(new IdentityRole(model.Name!));
                var error = CheckResponse(response);
                   
                if (!string.IsNullOrEmpty(error)) 
                    throw new Exception(error);

                else
                    return new GeneralResponse(true, $"{model.Name} created");
              
            }                
            return new GeneralResponse(false, $"{model.Name} already created");
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
    }
    
    private async Task<GeneralResponse> AssignUserToRole(ApplicationUser user, IdentityRole role)
    {
        if (user is null || role is null) return new GeneralResponse(false, "Model state cannot be done");
        if (await FindRoleByNameAsync(role.Name) == null)
            await CreateRoleAsync(role.Adapt(new CreateRoleDto()));

        IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name);
        string error = CheckResponse(result);
        if (!string.IsNullOrEmpty(error))
            return new GeneralResponse(false, error);
        else
            return new GeneralResponse(true, $"{user.Name} assigned to {role.Name} role");
    }
    
    public async Task<GeneralResponse> CreateAccountAsync(CreateAccountDto model)
    {
     try
 {
     if(await FindUserByEmailAsync(model.Email) != null)
         return new GeneralResponse(false, "User already exists!");


     int verficationCode = GenerateVerificationCode();
     
     var user = new ApplicationUser()
     {
         Name = model.Name,
         Email = model.Email,
         PasswordHash = model.Password,
         UserName = model.Email.Split('@')[0],
         VerificationCode = verficationCode.ToString(),
     };
     
     const string defaultRole = "Student";
     
     var result = await _userManager.CreateAsync(user, model.Password);
     
     string error = CheckResponse(result);
     
     if(!string.IsNullOrEmpty(error))
         return new GeneralResponse(false , error);
     
     var response = await AssignUserToRole(user, new IdentityRole{ Name = defaultRole});
     
     var tokenRequest = new GenerateTokenRequest()
     {
         Email = user.Email,
         Id = Guid.Parse(user.Id),
         Claims = new Dictionary<string, JsonElement>
         {
             { "Name", JsonDocument.Parse($"\"{user.Name}\"").RootElement }
             // Add other claims as needed
         }
     };

     string jwtToken = await GenerateTokenAsync(tokenRequest);
     string refreshToken = GenerateRefreshToken();
     
     if(string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
         return new GeneralResponse(false , "Error occured while logging in.  Please try again.");
     
     else
     {
         
         var saveResult = await SaveRefreshToken(user.Id, refreshToken);

         if (saveResult.Flag)
         {
             var mailRequest = new MailRequest()
             {
                 ToEmail = user.Email,
                 Subject = "Account Verification Code",
                 Body = $"Your verification code is {user.VerificationCode}, Please do not share it!"
             };

             var emailResult = await _emailRepository.SendEmailAsync(mailRequest, user.Id);
             if (emailResult.Flag)
                 return new GeneralResponse(true, "Account Create, Verification code sent to your email");

             else
                 return new GeneralResponse(false, "Please try again later!");
         }
         else
             return new GeneralResponse(false , "Error occured while logging in.  Please try again.");
     }
 }
 catch (Exception ex)
 {
     return new GeneralResponse(false , ex.Message);
 }
    }
    
    public async Task<GeneralResponse> VerifyAccountAsync(VerifyAccountDto model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return new GeneralResponse(false, "User does not exist");
          
            if(user.VerificationCode != model.VerificationCode)
                return new GeneralResponse(false,"Verification code does not match");

            user.EmailConfirmed = true;
            user.VerificationCode = null;

            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
                return new GeneralResponse(false , "Failed to verify account");
          
            return new GeneralResponse(true, "Account verified successfully");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(false, ex.Message);
        }
    }
    
    public async Task<GetUserDto?> GetUserAsync(string userId)
    {
        var getUser = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new GetUserDto(u.Id, u.Email, u.Name))
            .FirstOrDefaultAsync();

        return getUser;
    }
    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto model)
    {
        var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == model.Token);

        if (token == null) return new LoginResponse();
        var user = await _userManager.FindByIdAsync(token.UserId);

        var tokenRequest = new GenerateTokenRequest()
        {
            Email = user.Email,
            Id = Guid.Parse(user.Id),
            Claims = new Dictionary<string, JsonElement>
            {
                { "Name", JsonDocument.Parse($"\"{user.Name}\"").RootElement }
                // Add more claims if needed, like roles or custom data
            }
        };

        
        string newToken = await GenerateTokenAsync(tokenRequest);
        string newRefreshToken = GenerateRefreshToken();

        var saveResult = await SaveRefreshToken(user.Id, newRefreshToken);
        if (saveResult.Flag)
            return new LoginResponse(true, $"{user.Name} successfully re-logged in", newToken, newRefreshToken);
        else
            return new LoginResponse();
    }
    
    public async Task<GeneralResponse> SaveRefreshToken(string userId, string token)
    {
        try
        {
            var user = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if (user == null)
                _context.RefreshTokens.Add(new RefreshToken() { UserId = userId, Token = token });
            else
                user.Token = token;

            await _context.SaveChangesAsync();
            return new GeneralResponse(true, null!);
        }
        catch (Exception ex)
        {
            return new GeneralResponse(false, ex.Message);
        }
    }
    
    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    public async Task<string> GenerateTokenAsync(GenerateTokenRequest model)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, model.Email),
            new(JwtRegisteredClaimNames.Email, model.Email),
            new("Id", model.Id.ToString())
        };

        foreach (var claimPair in model.Claims)
        {
            var jsonElement = (JsonElement)claimPair.Value;
            var valueType = jsonElement.ValueKind switch
            {
                JsonValueKind.True => ClaimValueTypes.Boolean,
                JsonValueKind.False => ClaimValueTypes.Boolean,
                JsonValueKind.Number => ClaimValueTypes.Double,
                _ => ClaimValueTypes.String
            };

            var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);
            claims.Add(claim);
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            Issuer = "https://localhost:7201",
            Audience = "https://localhost:7201",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    
    private static string CheckResponse(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(_ => _.Description);
            return string.Join(Environment.NewLine, errors);
        }
        return null!;

    }
    
    private async Task<ApplicationUser> FindUserByEmailAsync(string email)
        => await _userManager.FindByEmailAsync(email);
  
    private async Task<ApplicationUser> FindUserByIdAsync(string userId)
        => await _userManager.FindByIdAsync(userId);
  
    private async Task<IdentityRole> FindRoleByNameAsync(string roleName)
        => await _roleManager.FindByNameAsync(roleName);
    private int GenerateVerificationCode()
    {
        return _random.Next(1000, 10000); // Generates a 4-digit random number
    }
    
}