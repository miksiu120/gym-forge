using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WorkPlanner.Configs;

public static class AuthenticationConfig
{
    public static IServiceCollection AddAuthenticationConfig(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Authentication");
        var settings = section.Get<AuthenticationSettings>()
            ?? throw new InvalidOperationException("Authentication settings are missing.");

        services
            .AddOptions<AuthenticationSettings>()
            .Bind(section)
            .Validate(
                options => options.JwtKey.Length >= 32,
                "Authentication:JwtKey must contain at least 32 characters.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.JwtIssuer),
                "Authentication:JwtIssuer is required.")
            .Validate(
                options => options.JwtExpireAccount > 0
                    && options.JwtRefreshTokenAccount > 0,
                "Authentication token lifetimes must be greater than zero.")
            .ValidateOnStart();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(settings.JwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = settings.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = settings.JwtIssuer,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}
