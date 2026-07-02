using WorkPlanner.Application.Exceptions;

namespace WorkPlanner.Api.ErrorHandling;

public readonly record struct ApiError(int StatusCode, string Title, string Detail);

public static class ApiExceptionMapper
{
    public static ApiError Map(Exception exception) => exception switch
    {
        BadRequestException => new(
            StatusCodes.Status400BadRequest,
            "Invalid request",
            exception.Message),
        InvalidCredentialsException => new(
            StatusCodes.Status401Unauthorized,
            "Authentication failed",
            exception.Message),
        UnauthorizedAccessException => new(
            StatusCodes.Status401Unauthorized,
            "Unauthorized",
            exception.Message),
        NotFoundException => new(
            StatusCodes.Status404NotFound,
            "Resource not found",
            exception.Message),
        ConflictException => new(
            StatusCodes.Status409Conflict,
            "Conflict",
            exception.Message),
        _ => new(
            StatusCodes.Status500InternalServerError,
            "An unexpected error occurred",
            "An unexpected error occurred. Use the trace id when contacting support.")
    };
}
