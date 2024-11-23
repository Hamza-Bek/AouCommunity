using Application.DTOs.Request.Entities;
using Domain.Models;
using FluentValidation;

namespace Application.Validators.Entities;

public class EventDtoValidator : AbstractValidator<EventDto>
{
    public EventDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .NotEmpty().WithMessage("Title is required")
            .Length(3,75).WithMessage("Title must be between 3 and 75 characters");

        RuleFor(x => x.Content)
            .Length(3, 500).WithMessage("Content must be between 3 and 500 characters");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow);

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate);
    }
}