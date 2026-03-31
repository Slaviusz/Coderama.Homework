namespace Coderama.Homework.DependencyInjection;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

public static class RegisterEndpointsExtensions {
    [RequiresUnreferencedCode("Dynamic REST API Endpoint registration")]
    public static void RegisterEndpoints(this IEndpointRouteBuilder app, ILogger logger)
    {
        Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && t is { IsAbstract: false })
            .Where(t => t.Name.EndsWith("Endpoint", StringComparison.Ordinal))
            .ToList()
            .ForEach(t => {
                var method = t
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(m => m.ReturnType == typeof(void));

                if (method == null) return;

                logger.LogInformation("Registering endpoint method {EndpointName}.{MethodName}", GetEndpointName(t), method.Name);
                method.Invoke(t, [app]);
            });
    }

    public static RouteHandlerBuilder WithEndpointNameTag(this RouteHandlerBuilder builder, Type type)
    {
        var name = GetEndpointName(type);
        builder.WithTags(name);
        return builder;
    }

    private static string GetEndpointName(Type type)
    {
        return $"{type.FullName?[..type.FullName.LastIndexOf('.')] ?? "Unknown"}." +
               $"{type.FullName?[(type.FullName.LastIndexOf("__", StringComparison.Ordinal) + 2)..] ?? "Unknown"}";
    }
}
