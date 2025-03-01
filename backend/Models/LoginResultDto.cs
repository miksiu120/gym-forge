namespace WorkPlanner.Models
{
    public class LoginResultDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        
        public string Nickname { get; set; }
        public string Message { get; set; } = "Successfully logged";

        
    }
}
