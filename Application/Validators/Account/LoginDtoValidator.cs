using Application.DTOs.Request.Account;
using FluentValidation;

namespace Application.Validators.Account;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .NotNull().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .NotNull().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}