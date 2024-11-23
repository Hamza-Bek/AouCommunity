using Application.DTOs.Request.Entities;
using Domain.Models;
using FluentValidation;

namespace Application.Validators.Entities;

public class OfferDtoValidator : AbstractValidator<OfferDto>
{
    public OfferDtoValidator()
    {
        RuleFor((x => x.Title))
            .NotEmpty().WithMessage("Title is required")
            .NotNull().WithMessage("Title is required");
        
        RuleFor(x => x.Content)
            .Length(5 , 500).WithMessage("Content must be between 5 and 500 characters");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .NotNull().WithMessage("Price is required");
        
    }
  
}