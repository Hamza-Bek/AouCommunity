using FluentValidation.Results;

namespace Application.DTOs.Response;

public record GeneralResponse(
    bool Flag = false,
    string Message = null!,
    string Token = null!,
    string RefreshToken = null!);