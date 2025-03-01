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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IHttpContextAccessorService,HttpContextAccessorService>();

builder.Services.AddAutoMapper(typeof(AccountMapper));
builder.Services.AddAutoMapper(typeof(AccountMapper).Assembly);

builder.Services.AddAuthenticationConfig(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseRouting(); 
app.UseCors("AllowFrontend"); 


app.UseAuthorization();

app.MapControllers();

app.Run();
