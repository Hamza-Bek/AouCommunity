using Application.DTOs.Request.Entities;
using FluentValidation;

namespace Application.Validators.Entities;

public class CommentDtoValidator : AbstractValidator<CommentDto>
{
    public CommentDtoValidator()
    {
        RuleFor((x => x.Content))
            .NotEmpty().WithMessage("Content is required")
            .NotNull().WithMessage("Content is required")
            .Length(1 , 200).WithMessage("Content must be between 1 and 200 characters");
    }
}