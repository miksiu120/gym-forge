namespace WorkPlanner.Configs
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public  int JwtExpireAccount { get; set; }
        public  int JwtRefreshTokenAccount { get; set; }
        public  string JwtIssuer { get; set; }
    }
}