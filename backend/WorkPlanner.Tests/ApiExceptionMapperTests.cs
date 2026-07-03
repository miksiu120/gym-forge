using WorkPlanner.Api.ErrorHandling;
using WorkPlanner.Application.Exceptions;

namespace WorkPlanner.Tests;

public sealed class ApiExceptionMapperTests
{
    [Theory]
    [InlineData(typeof(BadRequestException), StatusCodes.Status400BadRequest)]
    [InlineData(typeof(InvalidCredentialsException), StatusCodes.Status401Unauthorized)]
    [InlineData(typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized)]
    [InlineData(typeof(NotFoundException), StatusCodes.Status404NotFound)]
    [InlineData(typeof(ConflictException), StatusCodes.Status409Conflict)]
    [InlineData(typeof(InvalidOperationException), StatusCodes.Status500InternalServerError)]
    public void Map_returns_expected_status(Type exceptionType, int expectedStatus)
    {
        var exception = (Exception)Activator.CreateInstance(exceptionType, "test")!;

        var result = ApiExceptionMapper.Map(exception);

        Assert.Equal(expectedStatus, result.StatusCode);
    }

    [Fact]
    public void Unexpected_error_does_not_expose_exception_message()
    {
        var result = ApiExceptionMapper.Map(
            new InvalidOperationException("sensitive implementation detail"));

        Assert.DoesNotContain("sensitive", result.Detail);
    }
}
