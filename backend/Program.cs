using System.Text;
using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WorkPlanner.AutoMapper;
using WorkPlanner.Entities;
using WorkPlanner.Repositories;
using WorkPlanner.Repositories.PartyGame.Repositories;
using WorkPlanner.Services;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.IdentityModel.Tokens;
using PartyGame.Services;
using WorkPlanner.Configs;
using System.Text.Json;
using System.Text.Json.Serialization;
using WorkPlanner.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    })
//    .AddJwtBearer(cfg =>
//    {
//        cfg.RequireHttpsMetadata = false;
//        cfg.SaveToken = true;
//        cfg.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidIssuer = builder.Configuration["Authentication:JwtIssuer"],
//            ValidAudience = builder.Configuration["Authentication:JwtIssuer"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtKey"]))
//        };
//    });


builder.Services.AddDbContext<WorkPlannerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")),
    ServiceLifetime.Scoped);


builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<DatabaseSeeder>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITrainingPlanService, TrainingPlanService>();


builder.Services.AddScoped<IHttpContextAccessorService,HttpContextAccessorService>();

builder.Services.AddAutoMapper(typeof(AccountMapper));
builder.Services.AddAutoMapper(typeof(AccountMapper).Assembly);

builder.Services.AddAuthenticationConfig(builder.Configuration);


builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["ConnectionStrings:WebsiteConnection"] ?? "http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});




var app = builder.Build();

if (builder.Configuration.GetValue<bool>("ApplyMigrations") ||
    builder.Configuration.GetValue<bool>("SeedData"))
{
    using var scope = app.Services.CreateScope();
    var database = scope.ServiceProvider.GetRequiredService<WorkPlannerDbContext>();

    if (builder.Configuration.GetValue<bool>("ApplyMigrations"))
    {
        database.Database.Migrate();
    }

    if (builder.Configuration.GetValue<bool>("SeedData"))
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseRouting(); 
app.UseCors("AllowFrontend"); 


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
