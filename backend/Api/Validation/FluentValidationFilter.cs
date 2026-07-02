using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WorkPlanner.Api.Validation;

public sealed class FluentValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var failures = new Dictionary<string, List<string>>(StringComparer.Ordinal);

        foreach (var argument in context.ActionArguments.Values.Where(value => value is not null))
        {
            var validator = serviceProvider.GetService(
                typeof(IValidator<>).MakeGenericType(argument!.GetType())) as IValidator;
            if (validator is null)
            {
                continue;
            }

            var validationContextType = typeof(ValidationContext<>).MakeGenericType(argument.GetType());
            var validationContext = (IValidationContext)Activator.CreateInstance(
                validationContextType,
                argument)!;
            var result = await validator.ValidateAsync(
                validationContext,
                context.HttpContext.RequestAborted);

            foreach (var error in result.Errors)
            {
                if (!failures.TryGetValue(error.PropertyName, out var messages))
                {
                    messages = [];
                    failures.Add(error.PropertyName, messages);
                }

                if (!messages.Contains(error.ErrorMessage, StringComparer.Ordinal))
                {
                    messages.Add(error.ErrorMessage);
                }
            }
        }

        if (failures.Count == 0)
        {
            await next();
            return;
        }

        var problem = new ValidationProblemDetails(
            failures.ToDictionary(item => item.Key, item => item.Value.ToArray()))
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Instance = context.HttpContext.Request.Path
        };
        problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        context.Result = new BadRequestObjectResult(problem);
    }
}
