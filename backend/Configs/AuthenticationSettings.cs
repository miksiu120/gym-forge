namespace WorkPlanner.Configs
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; } = string.Empty;
        public int JwtExpireAccount { get; set; }
        public int JwtRefreshTokenAccount { get; set; }
        public string JwtIssuer { get; set; } = string.Empty;
    }
}
