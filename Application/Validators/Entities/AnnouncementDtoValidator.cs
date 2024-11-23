using Application.DTOs.Request.Entities;
using Domain.Models;
using FluentValidation;

namespace Application.Validators.Entities;

public class AnnouncementDtoValidator : AbstractValidator<AnnouncementDto>
{
    public AnnouncementDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .NotEmpty().WithMessage("Title is required")
            .Length(3,100).WithMessage("Title must be between 3 and 100 characters");
        
        RuleFor(x => x.Content)
            .Length(3 , 500).WithMessage("Content must be between 3 and 500 characters");
        
    }
}