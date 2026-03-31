namespace Coderama.Homework.Contracts.Validators;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        var entity = context.Arguments.OfType<T>().FirstOrDefault();

        if (validator is not null && entity is not null)
        {
            var result = await validator.ValidateAsync(entity);
            if (!result.IsValid) return TypedResults.ValidationProblem(result.ToDictionary());
        }

        return await next(context);
    }
}