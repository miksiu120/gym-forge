namespace WorkPlanner.Models;

public sealed class LoginResultDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Message { get; set; } = "Successfully logged";
}
