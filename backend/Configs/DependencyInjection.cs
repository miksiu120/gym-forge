using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkPlanner.Api.ErrorHandling;
using WorkPlanner.Api.Validation;
using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Accounts;
using WorkPlanner.Application.TrainingPlans;
using WorkPlanner.Data;
using WorkPlanner.Entities;
using WorkPlanner.Infrastructure.Identity;
using WorkPlanner.Infrastructure.Persistence;
using WorkPlanner.Services;

namespace WorkPlanner.Configs;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterAccount>();
        services.AddScoped<LoginAccount>();
        services.AddScoped<GetAccountDetails>();
        services.AddScoped<UpdateAccount>();

        services.AddScoped<CreateTrainingPlan>();
        services.AddScoped<GetTrainingPlans>();
        services.AddScoped<GetDueTrainingUnits>();
        services.AddScoped<GetTrainingStatistics>();
        services.AddScoped<GetTrainingUnit>();
        services.AddScoped<CompleteTrainingUnit>();

        services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>(
            ServiceLifetime.Transient);

        return services;
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<WorkPlannerDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DBConnection")));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITrainingPlanRepository, TrainingPlanRepository>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<DatabaseSeeder>();
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<FluentValidationFilter>();
        services
            .AddControllers(options =>
                options.Filters.AddService<FluentValidationFilter>())
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)));

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance ??= context.HttpContext.Request.Path;
                context.ProblemDetails.Extensions["traceId"] =
                    context.HttpContext.TraceIdentifier;
            };
        });
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddOpenApi();

        return services;
    }
}
