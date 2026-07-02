using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WorkPlanner.Configs;
using WorkPlanner.Data;
using WorkPlanner.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter(
    "Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware",
    LogLevel.Critical);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddApi()
    .AddAuthenticationConfig(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration["ConnectionStrings:WebsiteConnection"]
                ?? "http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("ApplyMigrations")
    || builder.Configuration.GetValue<bool>("SeedData"))
{
    using var scope = app.Services.CreateScope();
    var database = scope.ServiceProvider.GetRequiredService<WorkPlannerDbContext>();

    if (builder.Configuration.GetValue<bool>("ApplyMigrations"))
    {
        await database.Database.MigrateAsync();
    }

    if (builder.Configuration.GetValue<bool>("SeedData"))
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseStatusCodePages(async statusCodeContext =>
{
    await Results.Problem(
            statusCode: statusCodeContext.HttpContext.Response.StatusCode)
        .ExecuteAsync(statusCodeContext.HttpContext);
});
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
