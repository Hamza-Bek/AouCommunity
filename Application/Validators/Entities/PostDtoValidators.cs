using Application.DTOs.Request.Entities;
using FluentValidation;

namespace Application.Validators.Entities;

public class PostDtoValidators : AbstractValidator<PostDto>
{
    public PostDtoValidators()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .NotNull().WithMessage("Content is required")
            .Length(3, 2000).WithMessage("Content must be between 3 and 2000 characters");
    }
}