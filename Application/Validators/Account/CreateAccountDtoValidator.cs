using Application.DTOs.Request.Account;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Validators.Account;

public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .NotNull().WithMessage("Name is required")
            .Length(3 , 50).WithMessage("Name must be between 3 and 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .NotNull().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .NotNull().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("ConfirmPassword is required")
            .NotNull().WithMessage("ConfirmPassword is required")
            .Equal(x => x.Password).WithMessage("Passwords do not match");

    }
}