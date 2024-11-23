using Application.DTOs.Request.Account;
using Application.DTOs.Request.AuthenticationDto;
using Application.DTOs.Response.Account;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountsController : Controller
{
    private readonly IAccountRepository _accountRepository;
    private readonly IValidator<CreateAccountDto> _createAccountValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public AccountsController(IAccountRepository accountRepository, IValidator<LoginDto> loginValidator, IValidator<CreateAccountDto> createAccountValidator)
    {
        _accountRepository = accountRepository;
        _loginValidator = loginValidator;
        _createAccountValidator = createAccountValidator;
    }
    
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAccountAsync(CreateAccountDto model, CancellationToken cancellationToken)
    {
        var validationResult = await _createAccountValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var result = await _accountRepository.CreateAccountAsync(model);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto model, CancellationToken cancellationToken)
    {
        var validationResult = await _loginValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var result = await _accountRepository.LoginAccountAsync(model);
        return Ok(result);
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetUserAsync(string id)
    {
        var data = await _accountRepository.GetUserAsync(id);
        return Ok(data);
    }
    
    [HttpPost("verify/account")]
    public async Task<IActionResult> VerifyAccountAsync(VerifyAccountDto model)
    {
        var result = await _accountRepository.VerifyAccountAsync(model);
        return Ok(result);
    }

    [HttpPost("generate/token")]
    public async Task<ActionResult> GenerateToken(UserClaimsDto model)
    {
        return Ok(await _accountRepository.GenerateTokenAsync(model));
    }
}